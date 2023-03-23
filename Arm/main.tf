terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=3.47.0"
    }
    
  }
  backend "azurerm" {
    resource_group_name = "Wolford-AppService-WestEu-Dev"
    storage_account_name = "wolfordstoaccwesteudev"
    container_name = "terraformstatefile"
    key="wolford.trstate.dev"
    
  }
  
}

provider "azurerm" {
  skip_provider_registration = true
  features {
        key_vault {
            purge_soft_delete_on_destroy    = true
            recover_soft_deleted_key_vaults = true
          }
  }
}

data "azurerm_client_config" "current" {
}



variable "AdminUser"{
  type=string
  default="sadev"
}

variable "AdminPw"{
  type=string
  default="Password1!"
}

variable "SkuName"{
  type=string
  default="F1"
}

variable "ResGrName"{
  type=string
  default="Wolford-AppService-WestEu-Dev"
}
variable ResGrLocation{
  type=string
  default="West Europe"
}

/*
resource "azurerm_resource_group" "Wol_Rs_Group" {
    name     = "Wolford-AppService-WestEu-Dev"
  location = "West Europe"
}*/

resource "azurerm_service_plan" "Wol_AppSerPlann" {
   name="Wolford-AppServicePlan-WestEu-Dev"
   resource_group_name = var.ResGrName
   location = var.ResGrLocation
   os_type = "Windows"
   sku_name = var.SkuName
}

resource "azurerm_windows_web_app" "WolApis_WebApp" {
  name                = "wolfordapis-westeu-dev"
  resource_group_name = var.ResGrName
  location            = var.ResGrLocation
  service_plan_id     = azurerm_service_plan.Wol_AppSerPlan.id
    key_vault_reference_identity_id=azurerm_user_assigned_identity.Wol_ManagedIdentity.id
    identity {
      type="UserAssigned"
      identity_ids = [ azurerm_user_assigned_identity.Wol_ManagedIdentity.id ]
    }

  site_config {
    always_on =false
      application_stack{
    current_stack="dotnet"
    dotnet_version="v4.0"
    }


  }
    https_only=true
      connection_string {
    name="WolfordEmployeeConnectionString"
    type="SQLServer"
    value = "Server=tcp:${azurerm_mssql_server.Wol_SqlServer.name}.database.windows.net,1433;Initial Catalog=${azurerm_mssql_database.Wol_Db.name};Persist Security Info=False;User ID=${var.AdminUser};Password=${var.AdminPw};MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  }
}



resource "azurerm_mssql_server" "Wol_SqlServer" {
    name="wolford-sql-server-westeu-dev"
    resource_group_name=var.ResGrName
    location = var.ResGrLocation
    version="12.0"
    administrator_login =var.AdminUser
    administrator_login_password=var.AdminPw
     public_network_access_enabled = true

}

resource "azurerm_mssql_database" "Wol_Db" {
    name="wolfore-employee-sql-db-westeu-dev"
    server_id = azurerm_mssql_server.Wol_SqlServer.id
}

resource "azurerm_user_assigned_identity" "Wol_ManagedIdentity" {
    name="wolford-managedidentity-westeu-dev"
    location = var.ResGrLocation
    resource_group_name = var.ResGrName
}



resource "azurerm_mssql_firewall_rule" "ForAppAccess" {
  name                = "ForAppAccess"
  server_id         = azurerm_mssql_server.Wol_SqlServer.id
  start_ip_address    = "0.0.0.0"
  end_ip_address      = "255.255.255.255"
}

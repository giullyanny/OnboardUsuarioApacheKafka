# OboardUsuario
Link  youtube apresentando projeto: https://youtu.be/ATIiv8vhcF4

## Criar Banco de Dados e Tabela

```sql
-- Criar Banco de Dados e Tabela

use [master]
go

CREATE DATABASE onboard_user
GO

use onboard_user
go

CREATE TABLE [Users] (
    [Id] [int] IDENTITY(1, 1) NOT NULL
    , [Create] DATETIME NOT NULL
    , [Name] [nvarchar](100) NOT NULL
    , [Phone] [nvarchar](50) NOT NULL
    , [Password] [nvarchar](100) NOT NULL
    ) ON [PRIMARY]

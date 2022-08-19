Write-Host '-----------------------TRYING TO DELETE DATABASES  -----------------------'
try{
Invoke-SqlCmd -ServerInstance localhost -database master -Username "sa" -Password "Mosaico2021!" -query "
USE [master] 
GO 
IF EXISTS(select * from sys.databases where name='Mosaico')
ALTER DATABASE Mosaico SET SINGLE_USER WITH ROLLBACK IMMEDIATE 
GO
DROP DATABASE IF EXISTS Mosaico"
}catch {
    Write-Host "Mosaico Database does not exist"
}
try{
Invoke-SqlCmd -ServerInstance localhost -database master -Username "sa" -Password "Mosaico2021!" -query "
USE [master] 
GO 
IF EXISTS(select * from sys.databases where name='mosaicoid')
ALTER DATABASE mosaicoid SET SINGLE_USER WITH ROLLBACK IMMEDIATE 
GO
DROP DATABASE IF EXISTS mosaicoid"
}catch{
 Write-Host "mosaicoid Database does not exist"
}
try{
Invoke-SqlCmd -ServerInstance localhost -database master -Username "sa" -Password "Mosaico2021!" -query "
USE [master] 
GO 
IF EXISTS(select * from sys.databases where name='Tokenizer')
ALTER DATABASE Tokenizer SET SINGLE_USER WITH ROLLBACK IMMEDIATE 
GO
DROP DATABASE IF EXISTS Tokenizer"
}catch{
 Write-Host "Tokenizer Database does not exist"
}
Write-Host '-----------------------SUCCESSFULLY DELETED DATABASES-----------------------'
Write-Host '-----------------------TRYING TO CREATE DATABASES -----------------------'
Invoke-SqlCmd -ServerInstance localhost -database master -Username "sa" -Password "Mosaico2021!" -query "CREATE DATABASE Mosaico"
Invoke-SqlCmd -ServerInstance localhost -database master -Username "sa" -Password "Mosaico2021!" -query "CREATE DATABASE mosaicoid"
Invoke-SqlCmd -ServerInstance localhost -database master -Username "sa" -Password "Mosaico2021!" -query "CREATE DATABASE Tokenizer"
Write-Host '-----------------------SUCCESSFULLY CREATED -----------------------'
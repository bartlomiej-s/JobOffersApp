-- Connect to local database '(LocalDb)\MSSQLLocalDB'

USE [master]
GO
CREATE DATABASE [JobOfferDb] ON
( FILENAME = N'[Project directory]\JobOffersApp\WebApplication1\WebApplication1\Database\JobOfferDb.mdf' ),
( FILENAME = N'[Project directory]\JobOffersApp\WebApplication1\WebApplication1\Database\JobOfferDb_log.ldf' )
 FOR ATTACH
GO
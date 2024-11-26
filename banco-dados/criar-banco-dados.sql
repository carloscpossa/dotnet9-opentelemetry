CREATE DATABASE CidadesIbge
GO

USE CidadesIbge
GO

CREATE TABLE Cidades(
	CodigoIBGE char(7) NOT NULL,
	NomeCidade varchar(100) NOT NULL,
	Estado smallint NOT NULL,
	UF char(2) NOT NULL,

    CONSTRAINT PK_Cidade PRIMARY KEY (CodigoIBGE)
)
GO
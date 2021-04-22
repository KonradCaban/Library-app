CREATE PROCEDURE dbo.AddUser
    @pLogin NVARCHAR(50), 
    @pPassword NVARCHAR(50), 
    @pImie NVARCHAR(40), 
    @pNazwisko NVARCHAR(40),
	@pDataUr date,
	@pkod char(6),	
	@pmiasto varchar(30),
	@pULICA varchar(40),
	@pTELEFON int,	
    @responseMessage NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON

    BEGIN TRY

        INSERT INTO dbo.[czytelnicy] (LoginName, PasswordHash,rola,nazwisko,imie,dataur,kod,miasto,ulica,telefon)
        VALUES(@pLogin, HASHBYTES('SHA2_512', @pPassword),'uzytkownik',@pNazwisko,@pImie,@pDataUr,@pkod,@pmiasto,@pULICA,@pTELEFON)




        SET @responseMessage='Success'

    END TRY
    BEGIN CATCH
        SET @responseMessage=ERROR_MESSAGE() 
    END CATCH

END
	
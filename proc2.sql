CREATE PROCEDURE dbo.LoginInto 
    @pLoginName NVARCHAR(254),
    @pPassword NVARCHAR(50),
    @responseMessage NVARCHAR(250)='' OUTPUT,
	@usid int OUTPUT,
	@rola varchar(50) OUTPUT
	with encryption
AS
BEGIN

    SET NOCOUNT ON

    DECLARE @userID INT

    IF EXISTS (SELECT TOP 1 NRkarty FROM [dbo].[CZYTELNICY] WHERE LoginName=@pLoginName)
    BEGIN
        SET @userID=(SELECT NRkarty FROM [dbo].[CZYTELNICY] WHERE LoginName=@pLoginName AND  HASHBYTES('SHA2_512', @pPassword)=PasswordHash)
       IF(@userID IS NULL)	
           SET @responseMessage='Z≥y login bπdü has≥o.'
       ELSE 
	   begin
	   set @usid=@userID
       SET @responseMessage='Correct'
		SET @rola=(SELECT rola FROM [dbo].[CZYTELNICY] WHERE LoginName=@pLoginName AND  HASHBYTES('SHA2_512', @pPassword)=PasswordHash)
		end
    END
    ELSE
       SET @responseMessage='Z≥y login bπdü has≥o.'

END

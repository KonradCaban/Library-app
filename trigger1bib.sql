CREATE TRIGGER [dbo].[zmianaTytulu]
       ON [dbo].[Wykaz Publikacji]
       AFTER UPDATE
AS
BEGIN
       DECLARE @starak varchar(300) = (select tytul from deleted),
       @nowak varchar(300)=(select tytul from inserted)

       Insert into dbo.logs(message) values('Zmieniono nazwe ksiazki '+@starak+' na now¹: '+@nowak+' dnia '+ convert(varchar(50),GETDATE()))
END

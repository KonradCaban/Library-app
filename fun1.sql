CREATE FUNCTION dbo.pass (@input NVARCHAR(250),@id int)
RETURNS bit
AS BEGIN

    DECLARE @_p BINARY(64)
	Declare @tmp Binary(64)

    SET @_p =  HASHBYTES('SHA2_512', @Input)
    SET @tmp = (select PasswordHash from dbo.CZYTELNICY where NRkarty = @id)
	begin
	if @tmp=@_p
		RETURN 1;
	end
		return 0;
end;

set language polish
use master
drop database biblioteka
go
create database biblioteka
go

use biblioteka

create table AUTORZY
(
IDautora int Identity(1,1),
NAZWISKO varchar(30) not null,
IMIE varchar(30) not null,
UWAGI varchar(60) null,
Constraint PK_A_IDautora Primary Key (IDautora)
)

create table KATEGORIE
(
IDkategorii int Identity(1,1),
KATEGORIA varchar(35) not null,
Constraint PK_K_IDkategorii Primary Key (IDkategorii)
)


create table [WYKAZ WYDAWCOW]
(
IDwydawcy int Identity(1,1),
NAZWA varchar(50) not null,
Constraint PK_WW_IDwydawcy Primary Key (IDwydawcy)
)


create table [WYKAZ PUBLIKACJI]
(
IDksiazki int Identity(1,1),	-- klucz glowny
IDkategorii int not null,					-- klucz obcy
TYTUL varchar(40) not null,
IDwydawcy int not null,						-- klucz obcy
ROK int not null,							-- ograniczenie 4 liczby				-- klucz obcy
S£OWA_KLUCZOWE varchar(50) null,
Constraint PK_WP_IDksiazki Primary Key (IDksiazki)
)

ALTER TABLE [WYKAZ PUBLIKACJI]
ADD Constraint FK_WP_IDkategorii Foreign Key (IDkategorii) References KATEGORIE(IDKategorii)		-- klucz obcy

ALTER TABLE [WYKAZ PUBLIKACJI]
ADD Constraint FK_WP_IDwydawcy	Foreign Key (IDwydawcy) References [WYKAZ WYDAWCOW](IDwydawcy)		-- klucz obcy

ALTER TABLE [WYKAZ PUBLIKACJI]
ADD Constraint CK_WP_ROK CHECK (ROK LIKE '[1-2][0-9][0-9][0-9]')								-- sprawdzenie roku

create table posrednia_autor_ksiazka
(
idautora int not null,
idksiazki int not null
)


ALTER TABLE posrednia_autor_ksiazka
ADD Constraint PK_PAK_primary Primary Key (idautora,idksiazki)		-- 2 elementowy kl glowny

ALTER Table posrednia_autor_ksiazka
ADD Constraint FK_PAK_idautora Foreign Key (idautora) References AUTORZY(IDautora)				  --- klucz obcy

ALTER TABLE posrednia_autor_ksiazka
ADD Constraint FK_PAK_idksiazki Foreign Key (idksiazki) References [Wykaz Publikacji](IDksiazki)	-- klucz obcy


Create Table [WYKAZ EGZEMPLARZY]
(
IDegzemplarza int Identity(1,1),
IDksiazki int not null,
UBYTKI bit,
Constraint PK_WE_IDegzemplarza Primary Key (IDegzemplarza)
)

ALTER TABLE [Wykaz Egzemplarzy]
ADD Constraint DF_WE_Ubytki Default 0 for UBYTKI										-- default

ALTER TABLE [Wykaz Egzemplarzy]
ADD Constraint FK_WE_IDksiazki Foreign Key (IDksiazki) References [Wykaz Publikacji](IDksiazki)		-- klucz obcy

create table CZYTELNICY
(
LoginName NVARCHAR(40) NOT NULL UNIQUE,
rola nvarchar(20) default 'uzytkownik',
PasswordHash BINARY(64) NOT NULL,
NRkarty int identity(1,1),
NAZWISKO varchar(40) not null,
IMIE varchar(40) not null,
DataUr date not null,
kod char(6) not null,			-- check
miasto varchar(30) not null,
ULICA varchar(40) not null,
TELEFON int not null,		
Constraint PK_C_NRkarty Primary Key	(NRkarty)
)

 ALTER TABLE CZYTELNICY
 ADD CONSTRAINT CK_C_KOD CHECK ( KOD LIKE '[0-9][0-9]-[0-9][0-9][0-9]')				-- check

 create table [REJESTR WYPOZYCZEN]
 (
 NRkarty int not null,
 IDegzemplarza int not null,
 Datawyp date default getdate() not null,
 Zwrotprzed date null,
 DataZwrotu date default DateAdd(mm,3,getdate())
 )


  ALTER TABLE [REJESTR WYPOZYCZEN]
 ADD Constraint PK_RW_primary Primary Key (NRkarty,IDegzemplarza,Datawyp)		-- 3 elementowy kl glowny

 Alter Table [Rejestr Wypozyczen]
 ADD constraint FK_RW_NRkarty FOreign Key (NRkarty) References CZYTELNICY(NRkarty)	-- kl obcy

  Alter Table [Rejestr Wypozyczen]
 ADD constraint FK_RW_IDegzemplarza FOreign Key (IDegzemplarza) References [Wykaz Egzemplarzy](IDegzemplarza) -- kl obcy

 insert into CZYTELNICY(LoginName,rola,PasswordHash,NAZWISKO,imie,DataUr,kod,miasto,ulica,telefon) values('Admin','Administrator',HASHBYTES('SHA2_512', N'admin'),'Administrator','Pan',GETDATE(),'26-300','Miasto','jakas ulica','530059329')

 Create table logs
 (
 id_log int not null primary key identity(1,1),
 message varchar(500)
 )

 /*
select w.IDegzemplarza,w.IDksiazki,STUFF((
          SELECT ',' + IMIE +' ' + NAZWISKO 
          FROM AUTORZY
		 where a.idautora=AUTORZY.IDautora
          FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') from
 (
 select IDegzemplarza,IDksiazki from [WYKAZ EGZEMPLARZY]
 where ubytki = 0
 EXCEPT
 select w.IDegzemplarza,IDksiazki from [WYKAZ EGZEMPLARZY] as w join [REJESTR WYPOZYCZEN] as r on w.IDegzemplarza=r.IDegzemplarza
 where r.Zwrotprzed is null
 ) as w join [WYKAZ PUBLIKACJI] as k on k.IDksiazki=w.IDksiazki join posrednia_autor_ksiazka as a on a.idksiazki = k.IDksiazki 
*/
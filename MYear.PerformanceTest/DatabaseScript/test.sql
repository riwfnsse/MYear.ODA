CREATE TABLE [dbo].[Test](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[F_Byte] [tinyint] NULL,
	[F_Int16] [smallint] NULL,
	[F_Int32] [int] NULL,
	[F_Int64] [bigint] NULL,
	[F_Double] [float] NULL,
	[F_Float] [real] NULL,
	[F_Decimal] [decimal](18, 0) NULL,
	[F_Bool] [bit] NULL,
	[F_DateTime] [datetime] NULL,
	[F_Guid] [uniqueidentifier] NULL,
	[F_String] [nvarchar](100) NULL,
 CONSTRAINT [PK_Test] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

declare @i int = 0;

begin tran;
while(@i<=1000000)
begin
INSERT INTO [dbo].[Test]
           ([F_Byte]
           ,[F_Int16]
           ,[F_Int32]
           ,[F_Int64]
           ,[F_Double]
           ,[F_Float]
           ,[F_Decimal]
           ,[F_Bool]
           ,[F_DateTime]
           ,[F_Guid]
           ,[F_String])
     VALUES
           (1
           ,2
           ,@i
           ,@i
           ,@i
           ,@i
           ,@i
           ,@i%2
           ,GETDATE()
           ,NEWID()
           ,'Chloe' + CAST(@i AS nvarchar(1000))
		   )
set @i=@i+1;
end
commit;


CREATE TABLE SYS_USER
(
STATUS CHAR(1) DEFAULT 'O' NOT NULL,
CREATED_BY  VARCHAR(80),
CREATED_DATE DATETIME,
LAST_UPDATED_BY VARCHAR(80),
LAST_UPDATED_DATE DATETIME,
USER_ACCOUNT CHAR(36) NOT NULL,
USER_NAME VARCHAR(80) NOT NULL,
USER_PASSWORD VARCHAR(200)  NOT NULL,
EMAIL_ADDR VARCHAR(200),
PHONE_NO VARCHAR(80),
ADDRESS VARCHAR(200),
FE_MALE CHAR(1),
FAIL_TIMES INT  DEFAULT 0 NOT NULL,
IS_LOCKED CHAR(1) DEFAULT 'N' NOT NULL ,
PRIMARY KEY (USER_ACCOUNT)
);

CREATE TABLE SYS_USER_ROLE
(
STATUS CHAR(1)  DEFAULT 'O' NOT NULL,
CREATED_BY  VARCHAR(80),
CREATED_DATE DATETIME,
LAST_UPDATED_BY VARCHAR(80),
LAST_UPDATED_DATE DATETIME,
USER_ACCOUNT  CHAR(36)  NOT NULL,
ROLE_CODE CHAR(36)  NOT NULL,
PRIMARY KEY (USER_ACCOUNT,ROLE_CODE)
);
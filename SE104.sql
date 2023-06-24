﻿CREATE DATABASE SE104
DROP DATABASE SE104

USE SE104
SELECT CONVERT (date, GETDATE()) -- Without hours, minutes and seconds.

SET DATEFORMAT DMY


-------------------------------------------
--------------------CREATE TABLE

--------------------------------------------------------------------- NGUOI DUNG
CREATE TABLE NGDUNG (
	NGDUNG_ID INT IDENTITY(1,1) PRIMARY KEY,
	USERNAME NVARCHAR(255),
	EMAIL NVARCHAR(255),
	MATKHAU NVARCHAR(255),
)

SELECT * FROM NGDUNG
DBCC CHECKIDENT ('NGDUNG', RESEED, 3);

---------------------------------------------------------------------       HOC SINH
CREATE TABLE HOCSINH (
	HS_ID INT IDENTITY(1,1) PRIMARY KEY,
	LOP_ID INT,
	TENHS NVARCHAR(255),
	NGSINH SMALLDATETIME,
	DIACHI NVARCHAR(255),
	EMAIL NVARCHAR(255),
	GIOITINH BIT,
)

SELECT * FROM HOCSINH
DBCC CHECKIDENT ('HOCSINH', RESEED, 19);



---------------------------------------------------------------------         LOP HOC
CREATE TABLE LOP (
	LOP_ID INT IDENTITY(1,1)  PRIMARY KEY,
	KHOI_ID INT,
	TENLOP NVARCHAR(255),
)


SELECT * FROM LOP
DBCC CHECKIDENT ('LOP', RESEED, 7);

 
---------------------------------------------------------------------          KHOI
CREATE TABLE KHOI (
	KHOI_ID INT PRIMARY KEY,
	TENKHOI NVARCHAR(255),
)



---------------------------------------------------------------------           BANG DIEM
CREATE TABLE BANGDIEM (
	BD_ID INT IDENTITY(1,1) PRIMARY KEY,
	HK_ID INT ,
	HS_ID INT ,
	MH_ID INT ,
	DIEM15 FLOAT,
	DIEM45 FLOAT,
	DIEMHK FLOAT,
	DIEMTB FLOAT,
)

SELECT * FROM BANGDIEM
DBCC CHECKIDENT ('BANGDIEM', RESEED, 25);

---------------------------------------------------------------------               HOC KY
CREATE TABLE HOCKY (
	HK_ID INT PRIMARY KEY,
	TENHK NVARCHAR(255),
)


---------------------------------------------------------------------                 MON HOC
CREATE TABLE MONHOC (
	MH_ID INT IDENTITY(1,1) PRIMARY KEY,
	TENMH NVARCHAR(255),
)

SELECT * FROM MONHOC
DBCC CHECKIDENT ('MONHOC', RESEED, 4);


---------------------------------------------------------------------                  QUY DINH
CREATE TABLE QUYDINH (
	QD_ID INT PRIMARY KEY,
	TUOIMAX FLOAT,
	TUOIMIN FLOAT,
	SLMAX FLOAT,
	DIEMQUAMON FLOAT,
)


----------------------------------------------------------------
--------------------------CONSTRAINTS

ALTER TABLE HOCSINH
ADD CONSTRAINT FK_HOCSINH_LOP
FOREIGN KEY (LOP_ID)
REFERENCES LOP (LOP_ID);

ALTER TABLE LOP
ADD CONSTRAINT FK_LOP_KHOI
FOREIGN KEY (KHOI_ID)
REFERENCES KHOI (KHOI_ID);


ALTER TABLE BANGDIEM
ADD CONSTRAINT FK_BANGDIEM_HOCKY
FOREIGN KEY (HK_ID)
REFERENCES HOCKY (HK_ID);

ALTER TABLE BANGDIEM
ADD CONSTRAINT FK_BANGDIEM_HOCSINH
FOREIGN KEY (HS_ID)
REFERENCES HOCSINH (HS_ID);

ALTER TABLE BANGDIEM
ADD CONSTRAINT FK_BANGDIEM_MONHOC
FOREIGN KEY (MH_ID)
REFERENCES MONHOC (MH_ID);


----------------------------------------------------------------
--------------------------IDENTITY

SELECT * FROM MONHOC
DBCC CHECKIDENT ('MONHOC', RESEED, 4);



----------------------------------------------------------------
--------------------------INSERTS


INSERT INTO KHOI VALUES(1, '10');
INSERT INTO KHOI VALUES(2, '11');
INSERT INTO KHOI VALUES(3, '12');

INSERT INTO LOP (KHOI_ID, TENLOP) VALUES(1, '10a5');
INSERT INTO LOP (KHOI_ID, TENLOP) VALUES(1, '10a2');
INSERT INTO LOP (KHOI_ID, TENLOP) VALUES(1, '10a3');
INSERT INTO LOP (KHOI_ID, TENLOP) VALUES(2, '11a1');
INSERT INTO LOP (KHOI_ID, TENLOP) VALUES(2, '11a2');
INSERT INTO LOP (KHOI_ID, TENLOP) VALUES(3, '12a1');
INSERT INTO LOP (KHOI_ID, TENLOP) VALUES(3, '12a2');
DELETE FROM LOP WHERE LOP_ID = 9


INSERT INTO NGDUNG (USERNAME, EMAIL, MATKHAU) VALUES('admin', '21522289@gm.uit.edu.vn', '1');


INSERT INTO HOCKY VALUES(2, 'HK2');

INSERT INTO MONHOC VALUES('Hoa');

INSERT INTO HOCSINH (LOP_ID, TENHS, DIACHI, EMAIL, GIOITINH) VALUES (1, N'Nguyễn Hoàng Linh', N'TP.HCM', 'dev@gmail.com', 1)



DELETE FROM BANGDIEM WHERE MH_ID = 5
DELETE FROM MONHOC WHERE MH_ID = 5

UPDATE HOCSINH SET LOP_ID = NULL WHERE LOP_ID = 11
DELETE FROM LOP WHERE LOP_ID = 11


INSERT INTO LOP (TENLOP, KHOI_ID) VALUES ('10a4', 1)

SELECT * FROM BANGDIEM where hs_Id = 15
select * from hocsinh where hs_Id = 15

select * from ngdung


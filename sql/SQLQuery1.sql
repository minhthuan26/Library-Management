use QuanLiThuVien
go

create table NhanVien(
	ID varchar(128) primary key not null,
	Ho nvarchar(max) not null,
	Ten nvarchar(max) not null,
	NgaySinh DateTime not null,
	SoDienThoai varchar(20) not null,
	DiaChi nvarchar(max) not null,
	IDTaiKhoan int 
)
go
alter table NhanVien
add constraint FK_NV_TK foreign key (IDTaiKhoan) references TaiKhoan(ID)

create table TaiKhoan
(
	ID int identity(1,1) primary key not null,
	TenTaiKhoan varchar(max) not null,
	MatKhau varchar(max) not null,
	IDChucVu int
)
go
alter table TaiKhoan
add TrangThai nvarchar(max) not null
alter table TaiKhoan
add constraint FK_TK_CV foreign key (IDChucVu) references ChucVu(ID)

create table ChucVu
(
	ID int identity(1,1) primary key not null,
	TenChucVu nvarchar(max) not null
)
go

create table Sach
(
	ID varchar(128) primary key not null,
	TenSach nvarchar(max) not null,
	Gia float not null,
	NgayXuatBan DateTime not null,
	IDTacGia varchar(128),
	SoLuong int not null,
	IDTheLoai int
)
go
alter table Sach
add TrangThai nvarchar(max) not null
alter table Sach
add constraint FK_S_TL foreign key (IDTheLoai) references TheLoai(ID)
alter table Sach
add constraint FK_S_TG foreign key (IDTacGia) references TacGia(ID)

create table TheLoai
(
	ID int identity(1,1) primary key,
	TenTheLoai nvarchar(max)
)
go

create table TacGia
(
	ID varchar(128) primary key not null,
	Ho nvarchar(max) not null,
	Ten nvarchar(max) not null
)
go

create table KhachHang
(
	ID varchar(128) primary key not null,
	Ho nvarchar(max) not null,
	Ten nvarchar(max) not null,
	NgaySinh DateTime not null,
	SoDienThoai varchar(20) not null,
	DiaChi nvarchar(max) not null
)
go
alter table KhachHang
add TrangThai nvarchar(max) not null

create table PhieuMuon
(
	ID varchar(128) primary key not null,
	NgayLapPhieu DateTime not null
)
go
alter table PhieuMuon
add TrangThai nvarchar(max) not null

create table ChiTietPhieuMuon
(
	ID varchar(128) primary key not null,
	IDPhieuMuon varchar(128),
	IDSach varchar(128),
	IDKhachHang varchar(128),
	SoLuong int not null
)
go

alter table ChiTietPhieuMuon
add constraint FK_CTPM_PM foreign key (IDPhieuMuon) references PhieuMuon(ID)
alter table ChiTietPhieuMuon
add constraint FK_CTPM_S foreign key (IDSach) references Sach(ID)
alter table ChiTietPhieuMuon
add constraint FK_CTPM_KH foreign key (IDKhachHang) references KhachHang(ID)

create table PhieuBoiThuong
(
	ID varchar(128) primary key not null,
	NgayLapPhieu DateTime not null
)
go
alter table PhieuBoiThuong
add TrangThai nvarchar(max) not null
alter table PhieuBoiThuong
add IDPhieuMuon varchar(128) not null
alter table PhieuBoiThuong
add constraint FK_PBT_PM foreign key (IDPhieuMuon) references PhieuMuon(ID)

create table ChiTietPhieuBoiThuong
(
	ID varchar(128) primary key not null,
	IDPhieuBoiThuong varchar(128),
	IDSach varchar(128),
	IDKhachHang varchar(128),
	SoLuong int not null,
	Gia float not null
)
go
alter table ChiTietPhieuBoiThuong
add constraint FK_CTPBT_PM foreign key (IDPhieuMuon) references PhieuMuon(ID)
alter table ChiTietPhieuBoiThuong
add constraint FK_CTPBT_PBT foreign key (IDPhieuBoiThuong) references PhieuBoiThuong(ID)
alter table ChiTietPhieuBoiThuong
add constraint FK_CTPBT_S foreign key (IDSach) references Sach(ID)
alter table ChiTietPhieuBoiThuong
drop FK_CTPBT_KH
alter table ChiTietPhieuBoiThuong
add constraint FK_CTPBT_CTPM foreign key (IDKhachHang) references ChiTietPhieuMuon(IDKhachHang)

insert into ChucVu(TenChucVu) values(N'Admin')
insert into ChucVu(TenChucVu) values(N'Thủ thư')
insert into TaiKhoan(TenTaiKhoan, MatKhau, IDChucVu, TrangThai) 
values(N'Admin', 'db69fc039dcbd2962cb4d28f5891aae1', 1, '1')
insert into TaiKhoan(TenTaiKhoan, MatKhau, IDChucVu, TrangThai) 
values(N'Nv1', '9dfa104dd0102bb70fbc1124a271a6ae', 2, '1')
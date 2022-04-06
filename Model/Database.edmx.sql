
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/06/2022 21:01:18
-- Generated from EDMX file: D:\C#\learning\QuanLyThuVien\QuanLyThuVien\Model\Database.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [QuanLiThuVien];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CTPBT_PBT]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChiTietPhieuBoiThuong] DROP CONSTRAINT [FK_CTPBT_PBT];
GO
IF OBJECT_ID(N'[dbo].[FK_CTPBT_S]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChiTietPhieuBoiThuong] DROP CONSTRAINT [FK_CTPBT_S];
GO
IF OBJECT_ID(N'[dbo].[FK_CTPM_KH]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChiTietPhieuMuon] DROP CONSTRAINT [FK_CTPM_KH];
GO
IF OBJECT_ID(N'[dbo].[FK_CTPM_PM]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChiTietPhieuMuon] DROP CONSTRAINT [FK_CTPM_PM];
GO
IF OBJECT_ID(N'[dbo].[FK_CTPM_S]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChiTietPhieuMuon] DROP CONSTRAINT [FK_CTPM_S];
GO
IF OBJECT_ID(N'[dbo].[FK_NV_TK]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhanVien] DROP CONSTRAINT [FK_NV_TK];
GO
IF OBJECT_ID(N'[dbo].[FK_PBT_PM]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PhieuBoiThuong] DROP CONSTRAINT [FK_PBT_PM];
GO
IF OBJECT_ID(N'[dbo].[FK_S_TG]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Sach] DROP CONSTRAINT [FK_S_TG];
GO
IF OBJECT_ID(N'[dbo].[FK_S_TL]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Sach] DROP CONSTRAINT [FK_S_TL];
GO
IF OBJECT_ID(N'[dbo].[FK_TK_CV]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaiKhoan] DROP CONSTRAINT [FK_TK_CV];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ChiTietPhieuBoiThuong]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChiTietPhieuBoiThuong];
GO
IF OBJECT_ID(N'[dbo].[ChiTietPhieuMuon]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChiTietPhieuMuon];
GO
IF OBJECT_ID(N'[dbo].[ChucVu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChucVu];
GO
IF OBJECT_ID(N'[dbo].[KhachHang]', 'U') IS NOT NULL
    DROP TABLE [dbo].[KhachHang];
GO
IF OBJECT_ID(N'[dbo].[NhanVien]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NhanVien];
GO
IF OBJECT_ID(N'[dbo].[PhieuBoiThuong]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PhieuBoiThuong];
GO
IF OBJECT_ID(N'[dbo].[PhieuMuon]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PhieuMuon];
GO
IF OBJECT_ID(N'[dbo].[Sach]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sach];
GO
IF OBJECT_ID(N'[dbo].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sysdiagrams];
GO
IF OBJECT_ID(N'[dbo].[TacGia]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TacGia];
GO
IF OBJECT_ID(N'[dbo].[TaiKhoan]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaiKhoan];
GO
IF OBJECT_ID(N'[dbo].[TheLoai]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TheLoai];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ChiTietPhieuBoiThuongs'
CREATE TABLE [dbo].[ChiTietPhieuBoiThuongs] (
    [ID] varchar(128)  NOT NULL,
    [IDPhieuBoiThuong] varchar(128)  NULL,
    [IDSach] varchar(128)  NULL,
    [IDKhachHang] varchar(128)  NULL,
    [SoLuong] int  NOT NULL,
    [Gia] float  NOT NULL
);
GO

-- Creating table 'ChiTietPhieuMuons'
CREATE TABLE [dbo].[ChiTietPhieuMuons] (
    [ID] varchar(128)  NOT NULL,
    [IDPhieuMuon] varchar(128)  NULL,
    [IDSach] varchar(128)  NULL,
    [IDKhachHang] varchar(128)  NULL,
    [SoLuong] int  NOT NULL
);
GO

-- Creating table 'ChucVus'
CREATE TABLE [dbo].[ChucVus] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [TenChucVu] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'KhachHangs'
CREATE TABLE [dbo].[KhachHangs] (
    [ID] varchar(128)  NOT NULL,
    [Ho] nvarchar(max)  NOT NULL,
    [Ten] nvarchar(max)  NOT NULL,
    [NgaySinh] datetime  NOT NULL,
    [SoDienThoai] varchar(20)  NOT NULL,
    [DiaChi] nvarchar(max)  NOT NULL,
    [TrangThai] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'NhanViens'
CREATE TABLE [dbo].[NhanViens] (
    [ID] varchar(128)  NOT NULL,
    [Ho] nvarchar(max)  NOT NULL,
    [Ten] nvarchar(max)  NOT NULL,
    [NgaySinh] datetime  NOT NULL,
    [SoDienThoai] varchar(20)  NOT NULL,
    [DiaChi] nvarchar(max)  NOT NULL,
    [IDTaiKhoan] int  NULL
);
GO

-- Creating table 'PhieuBoiThuongs'
CREATE TABLE [dbo].[PhieuBoiThuongs] (
    [ID] varchar(128)  NOT NULL,
    [NgayLapPhieu] datetime  NOT NULL,
    [TrangThai] nvarchar(max)  NOT NULL,
    [IDPhieuMuon] varchar(128)  NOT NULL
);
GO

-- Creating table 'PhieuMuons'
CREATE TABLE [dbo].[PhieuMuons] (
    [ID] varchar(128)  NOT NULL,
    [NgayLapPhieu] datetime  NOT NULL,
    [TrangThai] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Saches'
CREATE TABLE [dbo].[Saches] (
    [ID] varchar(128)  NOT NULL,
    [TenSach] nvarchar(max)  NOT NULL,
    [Gia] float  NOT NULL,
    [NgayXuatBan] datetime  NOT NULL,
    [IDTacGia] varchar(128)  NULL,
    [SoLuong] int  NOT NULL,
    [IDTheLoai] int  NULL,
    [HinhAnh] varchar(max)  NULL
);
GO

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- Creating table 'TacGias'
CREATE TABLE [dbo].[TacGias] (
    [ID] varchar(128)  NOT NULL,
    [Ho] nvarchar(max)  NOT NULL,
    [Ten] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'TaiKhoans'
CREATE TABLE [dbo].[TaiKhoans] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [TenTaiKhoan] varchar(max)  NOT NULL,
    [MatKhau] varchar(max)  NOT NULL,
    [IDChucVu] int  NULL,
    [TrangThai] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'TheLoais'
CREATE TABLE [dbo].[TheLoais] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [TenTheLoai] nvarchar(max)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'ChiTietPhieuBoiThuongs'
ALTER TABLE [dbo].[ChiTietPhieuBoiThuongs]
ADD CONSTRAINT [PK_ChiTietPhieuBoiThuongs]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ChiTietPhieuMuons'
ALTER TABLE [dbo].[ChiTietPhieuMuons]
ADD CONSTRAINT [PK_ChiTietPhieuMuons]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ChucVus'
ALTER TABLE [dbo].[ChucVus]
ADD CONSTRAINT [PK_ChucVus]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'KhachHangs'
ALTER TABLE [dbo].[KhachHangs]
ADD CONSTRAINT [PK_KhachHangs]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'NhanViens'
ALTER TABLE [dbo].[NhanViens]
ADD CONSTRAINT [PK_NhanViens]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'PhieuBoiThuongs'
ALTER TABLE [dbo].[PhieuBoiThuongs]
ADD CONSTRAINT [PK_PhieuBoiThuongs]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'PhieuMuons'
ALTER TABLE [dbo].[PhieuMuons]
ADD CONSTRAINT [PK_PhieuMuons]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Saches'
ALTER TABLE [dbo].[Saches]
ADD CONSTRAINT [PK_Saches]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- Creating primary key on [ID] in table 'TacGias'
ALTER TABLE [dbo].[TacGias]
ADD CONSTRAINT [PK_TacGias]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TaiKhoans'
ALTER TABLE [dbo].[TaiKhoans]
ADD CONSTRAINT [PK_TaiKhoans]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TheLoais'
ALTER TABLE [dbo].[TheLoais]
ADD CONSTRAINT [PK_TheLoais]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [IDPhieuBoiThuong] in table 'ChiTietPhieuBoiThuongs'
ALTER TABLE [dbo].[ChiTietPhieuBoiThuongs]
ADD CONSTRAINT [FK_CTPBT_PBT]
    FOREIGN KEY ([IDPhieuBoiThuong])
    REFERENCES [dbo].[PhieuBoiThuongs]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CTPBT_PBT'
CREATE INDEX [IX_FK_CTPBT_PBT]
ON [dbo].[ChiTietPhieuBoiThuongs]
    ([IDPhieuBoiThuong]);
GO

-- Creating foreign key on [IDSach] in table 'ChiTietPhieuBoiThuongs'
ALTER TABLE [dbo].[ChiTietPhieuBoiThuongs]
ADD CONSTRAINT [FK_CTPBT_S]
    FOREIGN KEY ([IDSach])
    REFERENCES [dbo].[Saches]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CTPBT_S'
CREATE INDEX [IX_FK_CTPBT_S]
ON [dbo].[ChiTietPhieuBoiThuongs]
    ([IDSach]);
GO

-- Creating foreign key on [IDKhachHang] in table 'ChiTietPhieuMuons'
ALTER TABLE [dbo].[ChiTietPhieuMuons]
ADD CONSTRAINT [FK_CTPM_KH]
    FOREIGN KEY ([IDKhachHang])
    REFERENCES [dbo].[KhachHangs]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CTPM_KH'
CREATE INDEX [IX_FK_CTPM_KH]
ON [dbo].[ChiTietPhieuMuons]
    ([IDKhachHang]);
GO

-- Creating foreign key on [IDPhieuMuon] in table 'ChiTietPhieuMuons'
ALTER TABLE [dbo].[ChiTietPhieuMuons]
ADD CONSTRAINT [FK_CTPM_PM]
    FOREIGN KEY ([IDPhieuMuon])
    REFERENCES [dbo].[PhieuMuons]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CTPM_PM'
CREATE INDEX [IX_FK_CTPM_PM]
ON [dbo].[ChiTietPhieuMuons]
    ([IDPhieuMuon]);
GO

-- Creating foreign key on [IDSach] in table 'ChiTietPhieuMuons'
ALTER TABLE [dbo].[ChiTietPhieuMuons]
ADD CONSTRAINT [FK_CTPM_S]
    FOREIGN KEY ([IDSach])
    REFERENCES [dbo].[Saches]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CTPM_S'
CREATE INDEX [IX_FK_CTPM_S]
ON [dbo].[ChiTietPhieuMuons]
    ([IDSach]);
GO

-- Creating foreign key on [IDChucVu] in table 'TaiKhoans'
ALTER TABLE [dbo].[TaiKhoans]
ADD CONSTRAINT [FK_TK_CV]
    FOREIGN KEY ([IDChucVu])
    REFERENCES [dbo].[ChucVus]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TK_CV'
CREATE INDEX [IX_FK_TK_CV]
ON [dbo].[TaiKhoans]
    ([IDChucVu]);
GO

-- Creating foreign key on [IDTaiKhoan] in table 'NhanViens'
ALTER TABLE [dbo].[NhanViens]
ADD CONSTRAINT [FK_NV_TK]
    FOREIGN KEY ([IDTaiKhoan])
    REFERENCES [dbo].[TaiKhoans]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NV_TK'
CREATE INDEX [IX_FK_NV_TK]
ON [dbo].[NhanViens]
    ([IDTaiKhoan]);
GO

-- Creating foreign key on [IDPhieuMuon] in table 'PhieuBoiThuongs'
ALTER TABLE [dbo].[PhieuBoiThuongs]
ADD CONSTRAINT [FK_PBT_PM]
    FOREIGN KEY ([IDPhieuMuon])
    REFERENCES [dbo].[PhieuMuons]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PBT_PM'
CREATE INDEX [IX_FK_PBT_PM]
ON [dbo].[PhieuBoiThuongs]
    ([IDPhieuMuon]);
GO

-- Creating foreign key on [IDTacGia] in table 'Saches'
ALTER TABLE [dbo].[Saches]
ADD CONSTRAINT [FK_S_TG]
    FOREIGN KEY ([IDTacGia])
    REFERENCES [dbo].[TacGias]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_S_TG'
CREATE INDEX [IX_FK_S_TG]
ON [dbo].[Saches]
    ([IDTacGia]);
GO

-- Creating foreign key on [IDTheLoai] in table 'Saches'
ALTER TABLE [dbo].[Saches]
ADD CONSTRAINT [FK_S_TL]
    FOREIGN KEY ([IDTheLoai])
    REFERENCES [dbo].[TheLoais]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_S_TL'
CREATE INDEX [IX_FK_S_TL]
ON [dbo].[Saches]
    ([IDTheLoai]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
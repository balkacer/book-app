use master
go

drop database if exists libroapp
go

create database libroapp
go

use libroapp
go

create table author (
	[id] int identity(1, 1) primary key,
	[name] varchar (100) not null,
	[email] varchar (50) not null
)

create table category (
	[id] int identity(1, 1) primary key,
	[name] varchar (50) not null,
)

create table editorial (
	[id] int identity(1, 1) primary key,
	[name] varchar (50) not null,
	[phone] char (10) not null,
	[country] varchar (100) not null
)

create table book (
	[id] int identity(1, 1) primary key,
	[name] varchar (100) not null,
	[year] char (4) not null,
	[category_id] int foreign key references category(id) not null,
	[author_id] int foreign key references author(id) not null,
	[editorial_id] int foreign key references editorial(id) not null,
)
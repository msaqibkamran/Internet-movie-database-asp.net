
go 
--Create database movieDatabase
go 
use movieDatabase
go


create table [user]
(
userid int identity(1,1)  primary key,
username varchar(20) not null unique,
[password] varchar(50) not null,
contactnumber varchar(15),
dob date,
email varchar(30) not null unique, 
gender varchar(6),
agegroup varchar(30) not null,

)
go

create table actor(
actorid int identity(1,1) primary key,
actorname varchar(20),
gender varchar(10),
overview varchar(max),
imageurl varchar(max),
)

create table genre(
genreid int,
genrename varchar(20),
movieid int foreign key references movies(movieid) on delete set default  on update cascade,
)
go

create Table [admin](
adminid int identity(1,1) primary key,
adminname varchar(20) not null,
[password] varchar(40) not null,
)

Create table movies(
movieid int identity(1,1) primary key,
name varchar(50) not null unique,
release_date date,
duration varchar(15),
directorname varchar(15),
overview varchar(max),
pictureurl varchar(max),
trailerurl varchar(max),
)
go


create table actorInMovie(
id int identity(1,1) primary key,
actorid int foreign key references actor(actorid) on delete set default  on update cascade,
movieid int foreign key references movies(movieid) on delete set default  on update cascade,
)

go


Create table rating(
id int identity(1,1) primary key,
movieid int foreign key references movies(movieid) on delete set default  on update cascade,
userid int foreign key references [user](userid) on delete set default  on update cascade,
stars float,
)
go

create table comment(
commentid int identity(1,1) primary key,
movieid int foreign key references movies(movieid) on delete set default  on update cascade,
userid int foreign key references [user](userid) on delete set default  on update cascade,
commentstatement varchar(100),
)
go

create table watch_list(
id int identity(1,1) primary key,
userid int foreign key references [user](userid) on delete set default  on update cascade,
movieid int foreign key references movies(movieid) on delete set default  on update cascade,
)
go


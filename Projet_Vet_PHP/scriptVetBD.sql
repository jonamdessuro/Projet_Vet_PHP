create table listeanimaux(
id int not null auto_increment,
animaltype varchar(20) not null,
animalname varchar(20) not null,
color varchar(20) not null,
ownername varchar(20) not null,
age int not null,
weight int not null,
primary key (id)
);
select * from listeanimaux;
drop table listeanimaux;


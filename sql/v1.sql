drop table if exists user_roles;
drop table if exists users;
drop sequence if exists users_seq;
drop sequence if exists user_roles_seq;
commit;

create sequence users_seq;

create  table users (
id integer not null default nextval('users_seq'),
username varchar(45) not null,
password varchar(45) not null,
enabled boolean not null default true,
primary key (id));

create sequence user_roles_seq;

create table user_roles (
id integer not null default nextval('user_roles_seq'),
id_user integer not null references users(id),
role varchar(45) not null,
primary key (id));  

commit;

--crear usuario admin
insert into users(username,password,enabled)
    values ('admin','0', true);
     
insert into user_roles (id_user, role)
    values ((select id from users where username = 'admin'), 'ROLE_USER');
insert into user_roles (id_user, role)
    values ((select id from users where username = 'admin'), 'ROLE_ADMIN');
    
--crear usuario no-admin
insert into users(username,password,enabled)
    values ('user','0', true);
    
insert into user_roles (id_user, role)
    values ((select id from users where username = 'user'), 'ROLE_USER');    

commit;    
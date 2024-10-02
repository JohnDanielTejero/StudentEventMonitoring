DROP DATABASE IF EXISTS wam1_midterm_db; 
CREATE DATABASE wam1_midterm_db;
USE wam1_midterm_db;

CREATE TABLE students(
	student_number VARCHAR(255) UNIQUE PRIMARY KEY NOT NULL,
    first_name VARCHAR(255) NOT NULL,
    last_name VARCHAR(255) NOT NULL,
    program TEXT NOT NULL,
    year_level INT(1) NOT NULL
);

CREATE TABLE events(
 event_id INT AUTO_INCREMENT PRIMARY KEY NOT NULL,
 title TEXT NOT NULL,
 description TEXT NOT NULL,
 start_date DATETIME NOT NULL,
 end_date DATETIME NOT NULL,
 venue VARCHAR(255) NOT NULL,
 status VARCHAR(255) NOT NULL
);

CREATE TABLE attendances(
 student_number VARCHAR(255) NOT NULL,
 event_id INT AUTO_INCREMENT NOT NULL,
 timein DATETIME,
 timeout DATETIME,
 FOREIGN KEY(student_number) REFERENCES students(student_number),
 FOREIGN KEY(event_id) REFERENCES events(event_id),
 PRIMARY KEY(student_number, event_id)
);

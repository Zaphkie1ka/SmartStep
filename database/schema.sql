-- SmartStep: схема базы данных MySQL 8 (восстановлена по моделям приложения).
-- Запуск:  mysql -u root -p < database/schema.sql
-- Повторный запуск безопасен: существующие таблицы не пересоздаются.

CREATE DATABASE IF NOT EXISTS smartstep CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;
USE smartstep;

-- Пользователи приложения (педагоги). Колонка Password должна вмещать BCrypt-хеш (60 символов).
CREATE TABLE IF NOT EXISTS teachers (
    ID        INT AUTO_INCREMENT PRIMARY KEY,
    Login     VARCHAR(100) NOT NULL UNIQUE,
    Password  VARCHAR(100) NOT NULL
);

-- Приказы (используются при добавлении студента)
CREATE TABLE IF NOT EXISTS `Order` (
    ID    INT AUTO_INCREMENT PRIMARY KEY,
    Type  VARCHAR(200) NOT NULL,
    Date  DATE NOT NULL
);

CREATE TABLE IF NOT EXISTS Directions (
    ID           INT AUTO_INCREMENT PRIMARY KEY,
    Name         VARCHAR(200) NOT NULL,
    Description  TEXT NULL
);

CREATE TABLE IF NOT EXISTS `Groups` (
    ID             INT AUTO_INCREMENT PRIMARY KEY,
    Name           VARCHAR(100) NOT NULL,
    DirectionName  INT NOT NULL,
    FOREIGN KEY (DirectionName) REFERENCES Directions (ID)
);

CREATE TABLE IF NOT EXISTS Students (
    ID                INT AUTO_INCREMENT PRIMARY KEY,
    First_Name        VARCHAR(100) NOT NULL,
    Last_Name         VARCHAR(100) NOT NULL,
    Birthday          DATE NOT NULL,
    School            VARCHAR(200) NOT NULL,
    Class             VARCHAR(20)  NOT NULL,
    Address           VARCHAR(255) NOT NULL,
    ParentFirst_Name  VARCHAR(100) NOT NULL,
    ParentLast_Name   VARCHAR(100) NOT NULL,
    ParentNumber      VARCHAR(30)  NOT NULL,
    OrderNumber       INT NOT NULL,
    FOREIGN KEY (OrderNumber) REFERENCES `Order` (ID)
);

CREATE TABLE IF NOT EXISTS StudentInGroup (
    ID           INT AUTO_INCREMENT PRIMARY KEY,
    StudentName  INT NOT NULL,
    GroupName    INT NOT NULL,
    FOREIGN KEY (StudentName) REFERENCES Students (ID) ON DELETE CASCADE,
    FOREIGN KEY (GroupName)   REFERENCES `Groups` (ID) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Events (
    ID        INT AUTO_INCREMENT PRIMARY KEY,
    Name      VARCHAR(200) NOT NULL,
    Date      DATE NOT NULL,
    Location  VARCHAR(255) NOT NULL,
    Count     INT NOT NULL DEFAULT 0
);

-- Достижения: какой студент, в каком мероприятии, с каким результатом
CREATE TABLE IF NOT EXISTS Progress (
    ID           INT AUTO_INCREMENT PRIMARY KEY,
    StudentName  INT NOT NULL,
    EventName    INT NOT NULL,
    Result       VARCHAR(100) NOT NULL,
    FOREIGN KEY (StudentName) REFERENCES Students (ID) ON DELETE CASCADE,
    FOREIGN KEY (EventName)   REFERENCES Events (ID)   ON DELETE CASCADE
);

-- Справочник инструктажей по технике безопасности
CREATE TABLE IF NOT EXISTS Safety (
    ID           INT AUTO_INCREMENT PRIMARY KEY,
    Name         VARCHAR(200) NOT NULL,
    Description  TEXT NULL
);

-- Кто из студентов прошёл какой инструктаж и когда
CREATE TABLE IF NOT EXISTS StudentSafety (
    ID           INT AUTO_INCREMENT PRIMARY KEY,
    StudentName  INT NOT NULL,
    SafetyName   INT NOT NULL,
    Date         DATE NOT NULL,
    FOREIGN KEY (StudentName) REFERENCES Students (ID) ON DELETE CASCADE,
    FOREIGN KEY (SafetyName)  REFERENCES Safety (ID)   ON DELETE CASCADE
);

-- Начальные данные ---------------------------------------------------------

-- Первый пользователь: логин admin, пароль admin.
-- Приложение заменит пароль на BCrypt-хеш при первом успешном входе.
-- ВАЖНО: после первого входа смените пароль и/или создайте своего пользователя.
INSERT INTO teachers (Login, Password)
SELECT 'admin', 'admin' WHERE NOT EXISTS (SELECT 1 FROM teachers WHERE Login = 'admin');

-- Без хотя бы одного приказа нельзя добавить студента. Замените на реальные приказы.
INSERT INTO `Order` (Type, Date)
SELECT 'Приказ о зачислении (пример)', CURDATE() WHERE NOT EXISTS (SELECT 1 FROM `Order`);

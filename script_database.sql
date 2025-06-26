-- MySQL dump 10.13  Distrib 8.0.40, for Win64 (x86_64)
--
-- Host: localhost    Database: db_appontment_clinik
-- ------------------------------------------------------
-- Server version	8.0.40

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `appointment`
--

DROP TABLE IF EXISTS `appointment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `appointment` (
  `id_appointment` int NOT NULL AUTO_INCREMENT,
  `id_doctor` int DEFAULT NULL,
  `id_med_card` int DEFAULT NULL,
  `date_appointment` date DEFAULT NULL,
  `time_appointment` time DEFAULT NULL,
  `status_appointment` enum('В ожидании','Завершён') DEFAULT NULL,
  PRIMARY KEY (`id_appointment`),
  KEY `fk_appointment_doctor_idx` (`id_doctor`),
  KEY `fk_appointment_med_card_idx` (`id_med_card`),
  CONSTRAINT `fk_appointment_doctor` FOREIGN KEY (`id_doctor`) REFERENCES `doctor` (`id_doctor`),
  CONSTRAINT `fk_appointment_med_card` FOREIGN KEY (`id_med_card`) REFERENCES `users` (`id_med_card`)
) ENGINE=InnoDB AUTO_INCREMENT=36 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `appointment`
--

LOCK TABLES `appointment` WRITE;
/*!40000 ALTER TABLE `appointment` DISABLE KEYS */;
INSERT INTO `appointment` VALUES (12,1,12,'2025-06-23','09:00:00','Завершён'),(13,7,5,'2025-04-24','10:30:00','Завершён'),(14,7,11,'2025-04-22','11:00:00','Завершён'),(27,1,6,'2025-06-25','08:30:00','Завершён'),(28,1,3,'2025-06-25','12:00:00','Завершён'),(29,1,1,'2025-06-25','13:45:00','Завершён'),(30,1,115,'2025-06-25','08:15:00','Завершён'),(31,1,16,'2025-06-25','11:00:00','Завершён'),(32,1,11,'2025-06-25','11:45:00','Завершён'),(33,1,4,'2025-06-25','14:00:00','Завершён'),(34,1,9,'2025-06-25','12:30:00','Завершён'),(35,1,115,'2025-06-25','09:00:00','В ожидании');
/*!40000 ALTER TABLE `appointment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `doctor`
--

DROP TABLE IF EXISTS `doctor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `doctor` (
  `id_doctor` int NOT NULL AUTO_INCREMENT,
  `id_specialty` int NOT NULL,
  `surname_doctor` varchar(50) NOT NULL,
  `name_doctor` varchar(50) NOT NULL,
  `patronymic_doctor` varchar(50) DEFAULT NULL,
  `email_doctor` varchar(100) NOT NULL,
  `phone_number_doctor` varchar(11) NOT NULL,
  `medical_experience` int DEFAULT NULL,
  `cabinet_number` varchar(6) DEFAULT NULL,
  `status_work` varchar(45) DEFAULT NULL,
  `icon_doctor` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id_doctor`),
  KEY `fk_doctor_shedule_idx` (`id_specialty`),
  KEY `fk_doctor_shedule_idx1` (`id_doctor`),
  CONSTRAINT `fk_doctor_specialty` FOREIGN KEY (`id_specialty`) REFERENCES `specialty` (`id_specialty`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `doctor`
--

LOCK TABLES `doctor` WRITE;
/*!40000 ALTER TABLE `doctor` DISABLE KEYS */;
INSERT INTO `doctor` VALUES (1,1,'Быкова','Лариса','Петровна','larisa@mail.ru','79613601152',6,'103','В отпуске','larisa.jpg'),(2,7,'Кузнецов','Леонид','Андреевич','smirnovv02@yandex.ru','79063641151',15,'133','В отпуске','leonid.jpg'),(3,6,'Морозова','Татьяна','Владимировна','moroz_t23@mail.ru','79870534611',10,'101А','По графику','moroz.jpg'),(4,10,'Комаров','Юрий','Олегович','komar_ov22@gmail.com','79053602849',6,'135','По графику','komarov.jpg'),(5,5,'Попова','Мария','Кирилловна','popova_m@clinic.ru','79261112233',8,'201','В отпуске','popova.jpg'),(6,9,'Смирнова','Василиса','Валерьевна','vasilisa.metastazova@hospital.ru','79154445566',12,'205','По графику','metastazova.jpg'),(7,2,'Волкова','Екатерина','Дмитриевна','volkova_e@medcenter.ru','79677778899',7,'112','По графику','zaitseva.jpg'),(8,3,'Крюкова','Ксения','Романовна','ksenia.krukova@zdorov.ru','79038889900',9,'211','Б/л','krukova.jpg'),(9,1,'Филинова','Ольга','Сергеевна','filin_olya@mail.ru','79463754483',10,'103','По графику','filinova.jpg');
/*!40000 ALTER TABLE `doctor` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `schedule`
--

DROP TABLE IF EXISTS `schedule`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `schedule` (
  `id_sсhedule` int NOT NULL AUTO_INCREMENT,
  `id_doctor` int NOT NULL,
  `day_week` enum('Пн','Вт','Ср','Чт','Пт','Сб','Вс') NOT NULL,
  `time_start` time NOT NULL,
  `time_end` time NOT NULL,
  PRIMARY KEY (`id_sсhedule`),
  KEY `idx_id_doctor` (`id_doctor`),
  CONSTRAINT `fk_schedule_doctor` FOREIGN KEY (`id_doctor`) REFERENCES `doctor` (`id_doctor`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `schedule`
--

LOCK TABLES `schedule` WRITE;
/*!40000 ALTER TABLE `schedule` DISABLE KEYS */;
INSERT INTO `schedule` VALUES (1,1,'Пн','08:00:00','14:00:00'),(2,1,'Вт','08:00:00','14:00:00'),(3,1,'Ср','08:00:00','14:00:00'),(4,1,'Чт','08:00:00','14:00:00'),(5,1,'Пт','08:00:00','14:00:00'),(6,9,'Пн','14:00:00','19:00:00'),(7,9,'Вт','14:00:00','19:00:00'),(8,9,'Ср','14:00:00','19:00:00'),(9,9,'Чт','14:00:00','19:00:00'),(10,9,'Пт','14:00:00','19:00:00'),(11,2,'Пн','16:00:00','20:00:00'),(12,2,'Ср','13:00:00','17:00:00'),(13,2,'Пт','13:00:00','17:00:00'),(14,3,'Пн','16:00:00','20:00:00'),(15,3,'Ср','13:00:00','17:00:00'),(16,3,'Чт','16:00:00','20:00:00'),(17,7,'Пн','08:00:00','15:00:00'),(18,7,'Вт','08:00:00','15:00:00'),(19,7,'Ср','08:00:00','15:00:00'),(20,7,'Чт','08:00:00','15:00:00'),(21,7,'Пт','08:00:00','15:00:00'),(22,5,'Пн','08:00:00','14:00:00');
/*!40000 ALTER TABLE `schedule` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `specialty`
--

DROP TABLE IF EXISTS `specialty`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `specialty` (
  `id_specialty` int NOT NULL AUTO_INCREMENT,
  `name_specialty` varchar(50) NOT NULL,
  `time_accept` int DEFAULT NULL,
  `icon_specialty` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id_specialty`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `specialty`
--

LOCK TABLES `specialty` WRITE;
/*!40000 ALTER TABLE `specialty` DISABLE KEYS */;
INSERT INTO `specialty` VALUES (1,'Терапевт',15,'therapist.png'),(2,'Педиатр',20,'pediatry.png'),(3,'Офтальмолог',15,'eyes.png'),(4,'Отоларинголог',20,'lor.png'),(5,'Дерматовенеролог',20,'dermatology.png'),(6,'Травматолог',35,'trauma.png'),(7,'Хирург',25,'surgery.png'),(8,'Аллерголог',35,'allergy.png'),(9,'Онколог',25,'oncology.png'),(10,'Кардиолог',25,'cardiology.png'),(11,'Уролог',30,'urology.png'),(12,'Акушер-гинеколог',30,'ginecolog.png');
/*!40000 ALTER TABLE `specialty` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `id_med_card` int NOT NULL AUTO_INCREMENT,
  `email_users` varchar(100) NOT NULL,
  `role_id_users` enum('Администратор','Пользователь') NOT NULL,
  `surname_users` varchar(50) NOT NULL,
  `name_users` varchar(50) NOT NULL,
  `date_birth` date DEFAULT NULL,
  `medical_policy` varchar(16) DEFAULT NULL,
  `passport_number` varchar(10) DEFAULT NULL,
  `phone_number` varchar(11) DEFAULT NULL,
  PRIMARY KEY (`id_med_card`)
) ENGINE=InnoDB AUTO_INCREMENT=124 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'ivanova@yandex.ru','Пользователь','Иванова','Гульсум','1978-01-01','1748339956337720','8020234562','79064374858'),(2,'alexandr@yandex.ru','Пользователь','Иванов','Александр','2000-01-02','1233454234334620','8020234563','79064833374'),(3,'zhukov.s@mail.ru','Пользователь','Жуков','Сергей','1978-01-03','7826723554354340','8015012345','79161234567'),(4,'kondratiev.e@gmail.com','Пользователь','Кондратьев','Егор','1978-01-04','3456374857347840','8010678901','79252345678'),(5,'denisova.d@yandex.ru','Пользователь','Денисова','Дарья','2000-01-05','4567578348738820','8060678912','79033456789'),(6,'kuzmina.p@mail.ru','Пользователь','Кузьмина','Полина','2006-01-06','2275649956320080','8016758656','79674567890'),(7,'scherbakov.t@gmail.com','Пользователь','Щербаков','Тимофей','2007-01-07','5473368566392770','8010345345','79855678901'),(9,'rybakova.a@mail.ru','Пользователь','Рыбакова','Ангелина','1978-01-09','7473488846226480','8056748932','79167890123'),(10,'kalinina.v@gmail.com','Пользователь','Калинина','Варвара','1977-01-10','6657363387844770','8022643543','79268901234'),(11,'romanova.m@yandex.ru','Пользователь','Романова','Милана','2006-01-11','8988472611874620','8030584783','79039012345'),(12,'kalinin.z@mail.ru','Пользователь','Калинин','Захар','1978-01-12','6563846577274570','8010678434','79670123456'),(13,'belyaeva.t@gmail.com','Пользователь','Беляева','Таисия','2006-01-13','6658363437594470','8010645344','79851234567'),(14,'habirova.a@yandex.ru','Пользователь','Хабирова','Айгуль','2007-01-14','9782747399943820','8010635343','79052345678'),(15,'zaripov.a@mail.ru','Пользователь','Зарипов','Айдар','1977-01-15','2284829473947330','8010678345','79163456789'),(16,'ilmira@yandex.ru','Пользователь','Батыршина','Ильмира','1977-01-16','3454534434234520','8020103764','79613629956'),(115,'reginasafina0227@gmail.com','Администратор','Сафина','Регина','2006-02-27','3445345434534534','8020103734','');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-06-26 15:40:20

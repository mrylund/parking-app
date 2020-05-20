-- --------------------------------------------------------
-- Host:                         rylund.dev
-- Server version:               10.4.13-MariaDB - MariaDB Server
-- Server OS:                    Linux
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


CREATE DATABASE IF NOT EXISTS `DATABASENAME` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci */;
USE `DATABASENAME`;

CREATE TABLE IF NOT EXISTS `guests` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `LicensePlate` varchar(45) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Resident` int(11) NOT NULL DEFAULT 0,
  `created` timestamp NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID` (`ID`,`LicensePlate`),
  UNIQUE KEY `ID_2` (`ID`),
  KEY `resident_guest` (`Resident`),
  CONSTRAINT `resident_guest` FOREIGN KEY (`Resident`) REFERENCES `users` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;


CREATE TABLE IF NOT EXISTS `users` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(45) COLLATE utf8mb4_unicode_ci NOT NULL,
  `password` varchar(256) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Username` varchar(45) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Room` varchar(45) COLLATE utf8mb4_unicode_ci NOT NULL,
  `LicensePlate` varchar(45) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `Rank` varchar(45) COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT 'Resident',
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Username_UNIQUE` (`Username`),
  UNIQUE KEY `Room_UNIQUE` (`Room`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` (`ID`, `Name`, `password`, `Username`, `Room`, `LicensePlate`, `Rank`) VALUES
	(1, 'Admin', '8C6976E5B5410415BDE908BD4DEE15DFB167A9C873FC4BB8A81F6F2AB448A918', 'admin', '0', NULL, 'Admin');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;

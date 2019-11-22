-- phpMyAdmin SQL Dump
-- version 4.8.3
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 05-06-2019 a las 16:53:00
-- Versión del servidor: 10.1.36-MariaDB
-- Versión de PHP: 5.6.38

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `damnificus_usuarios`
--
CREATE DATABASE IF NOT EXISTS `damnificus_usuarios` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `damnificus_usuarios`;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `credenciales`
--

DROP TABLE IF EXISTS `credenciales`;
CREATE TABLE `credenciales` (
  `Id` int(5) NOT NULL,
  `Token` varchar(128) DEFAULT NULL,
  `Rango` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Volcado de datos para la tabla `credenciales`
--

INSERT INTO `credenciales` (`Id`, `Token`, `Rango`) VALUES
(1, '', 'admin'),
(2, '', 'admin'),
(3, '', 'admin'),
(4, '', 'admin'),
(5, '', 'normal'),
(6, '', 'normal'),
(7, '', 'normal'),
(8, '', 'normal'),
(9, '', 'normal'),
(10, '', 'normal'),
(11, '', 'normal');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

DROP TABLE IF EXISTS `usuarios`;
CREATE TABLE `usuarios` (
  `Id` int(5) NOT NULL,
  `usuario` varchar(32) NOT NULL,
  `email` varchar(89) NOT NULL,
  `pass` varchar(128) NOT NULL,
  `nombre` varchar(64) NOT NULL,
  `apellidos` varchar(64) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`Id`, `usuario`, `email`, `pass`, `nombre`, `apellidos`) VALUES
(1, 'admin', 'damnificusjovellanos@gmail.com', '8318/hk+2g+7kkJ99y+fWTZNI+SelwVUopFulvwfnwG56DLM1XVhe/jFejHg4nOMDo3yLRwcio+zdkL/pGEOrI4ehOwwnA==', 'administrador', 'damnificus'),
(2, 'antonio', 'antonioillarramendirubio@gmail.com', 'q6GaBrwGTdh+BA93gK+8UupE6CMqiSkLbsT4BhEvqycMkPTh3kYEm4/Gq6wjB22AxIRIMBWCAP6CMBE2Dxy4kvq1M5rF/g==', 'Antonio', 'Illarramendi'),
(3, 'ruben', 'rubenbalsera@cambialo.com', 'q2Cihwg3kPb+ls9WeQjvIei3m+JzfOGVx30tf0m6um9nMQYQC9ndn3DXpXTI935dK6XbeKCTcbzxCkeso0Oy+n48AvYhXQ==', 'Rubén', 'Balsera'),
(4, 'valentin', 'valentinsanchez@cambialo.com', 'DQYjDZjMrDekq/QezihoI9EO8YMsHabKBOcAkmjgmsFRbpOxQge+gQ5hzroUKtK31AEOqoIaoUXtXljPuLT3HMZB0PKRIw==', 'Valentín', 'Sanchez'),
(5, 'ana', 'anagonzalez@prueba.com', 'WOBRw57b7xGRcKeaBtBzXD0Z/521GoIRXfaJkgStLk8BifX0DOHCWuoE17HeugdyrWJvsnCoKttpLqH5GJrJGdrv2GA=', 'Ana', 'Gonzalez'),
(6, 'maria', 'mariagallardo@prueba.com', '39gkqi2Ia4DsAsvLo7OKUy/FH5t8JagyeRFyqGn5KeNJrn3Oz7XgCD5pj3r86Ek5YyOCt1e+kWdafpg5A3mp/jAny8jH2Q==', 'maria', 'gallardo'),
(7, 'juan', 'juanperez@prueba.com', 'ZduyM0KvqGPoHzbZPaO/C3UmrJoK/gcJ1v2xoHlt+1gp4XkG5kQrnTDICnOR6zBLDw3FKIYLWgBhUi8eHQkxhnRLIHSfcZc=', 'juan', 'perez'),
(8, 'susana', 'susanaromero@prueba.com', 'oqdR4V95Q3qukjMMHZWCmYQF+AQD7uFnc5CDvQKnHjSpMj6A2L5hEu9AWZErrCwE4rZMOC9Tr+ykg0E/p4p6ERzRgn0zkCc=', 'susana', 'romero'),
(9, 'carlos', 'carlosmartin@prueba.com', 'd2uLTxXv8UwzXn85t15LOxMTXXi3paIyyWeXJmmotQ4QG7wYSEupBUdte7KVj0+7gEy655yyV06vXZtAiXEsVexiyj8LFZo=', 'carlos', 'martín'),
(10, 'jessica', 'jessicaalbarracin@prueba.com', 'gjr3RERl1shS48bE4eNGMnkDAW8TC5jLhU36SxRgVYU0gCKb9KmqHTK0RAJq1fT+luT/iPeBJD9dpQ8JUx3An+NChLT8', 'jessica', 'albarracin'),
(11, 'pablo', 'pablomora@prueba.com', 'XGtxIHJ6j5R+N1T6Rsum7zRKpzjP3aJmiLEzz5qefYlvDLkcgFKWAHexxyDoREqYUB3jGF5P3jbmm2lMkyzqKDsGi+QFwg==', 'pablo', 'mora');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `credenciales`
--
ALTER TABLE `credenciales`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `Id` (`Id`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `usuario` (`usuario`),
  ADD UNIQUE KEY `email` (`email`),
  ADD UNIQUE KEY `Id` (`Id`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `credenciales`
--
ALTER TABLE `credenciales`
  MODIFY `Id` int(5) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `Id` int(5) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `credenciales`
--
ALTER TABLE `credenciales`
  ADD CONSTRAINT `Id` FOREIGN KEY (`Id`) REFERENCES `usuarios` (`Id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

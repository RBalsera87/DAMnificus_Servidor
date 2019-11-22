-- phpMyAdmin SQL Dump
-- version 4.8.3
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 05-06-2019 a las 16:52:19
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
-- Base de datos: `damnificus_enlaces`
--
CREATE DATABASE IF NOT EXISTS `damnificus_enlaces` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `damnificus_enlaces`;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `asignaturas`
--

DROP TABLE IF EXISTS `asignaturas`;
CREATE TABLE `asignaturas` (
  `Id` int(1) NOT NULL,
  `Nombre` varchar(128) NOT NULL,
  `Curso` int(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Volcado de datos para la tabla `asignaturas`
--

INSERT INTO `asignaturas` (`Id`, `Nombre`, `Curso`) VALUES
(1, 'Bases de datos', 1),
(2, 'Entornos de desarrollo', 1),
(3, 'Lenguajes de marcas', 1),
(4, 'Programación', 1),
(5, 'Sistemas informáticos', 1),
(6, 'Acceso a datos', 2),
(7, 'Desarrollo de interfaces', 2),
(8, 'Sistemas de gestión empresarial', 2),
(9, 'Programación de servicios y procesos', 2),
(10, 'Programación multimedia y dispositivos móviles', 2),
(11, 'Formación y orientación laboral', 1),
(12, 'Empresa e iniciativa emprendedora', 2),
(13, 'Inglés técnico', 2);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `curso`
--

DROP TABLE IF EXISTS `curso`;
CREATE TABLE `curso` (
  `Id` int(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Volcado de datos para la tabla `curso`
--

INSERT INTO `curso` (`Id`) VALUES
(0),
(1),
(2);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `enlaces`
--

DROP TABLE IF EXISTS `enlaces`;
CREATE TABLE `enlaces` (
  `Id` int(3) NOT NULL,
  `Link` varchar(512) NOT NULL,
  `Titulo` varchar(64) NOT NULL,
  `Descripcion` varchar(256) NOT NULL,
  `Valoracion` int(3) NOT NULL,
  `Imagen` varchar(512) NOT NULL,
  `Tipo` varchar(25) NOT NULL,
  `Tema` int(2) NOT NULL,
  `Uploader` int(5) NOT NULL,
  `Activo` int(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Volcado de datos para la tabla `enlaces`
--

INSERT INTO `enlaces` (`Id`, `Link`, `Titulo`, `Descripcion`, `Valoracion`, `Imagen`, `Tipo`, `Tema`, `Uploader`, `Activo`) VALUES
(1, 'https://www.youtube.com/watch?v=coK4jM5wvko&vl=es', 'Curso Java. Presentación. Vídeo ', 'Presentamos en este vídeo las características y el contenido del nuevo curso de Java que comienza en el canal.', 100, 'https://diylogodesigns.com/wp-content/uploads/2017/07/java-logo-vector-768x768.png', 'Video', 29, 1, 1),
(2, 'https://www.youtube.com/watch?v=F0ILFYl8YgI', 'Curso Java. Instalación JRE y Ec', 'Antes de comenzar a programar tenemos que instalar el software a utilizar en el curso y es lo que hacemos en este vídeo. Instala', 76, 'https://diylogodesigns.com/wp-content/uploads/2017/07/java-logo-vector-768x768.png', 'Video', 30, 2, 1),
(3, 'https://www.youtube.com/watch?v=UID_EKKfpcE', 'Curso Java Arrays I', 'Comenzamos en este vídeo a ver el tema de los Arrays, (matrices o arreglos). Vemos qué son exactamente y cuál es su sintaxis', 98, 'https://diylogodesigns.com/wp-content/uploads/2017/07/java-logo-vector-768x768.png', 'Video', 30, 3, 2),
(4, 'https://www.youtube.com/watch?v=Ng0_7uZyIoA', 'Curso Java. Entrada Salida datos', 'En este vídeo vemos cómo cambiar el flujo de datos seguido hasta ahora y conseguir introducir datos en nuestros programas. Para ', 71, '', 'Video', 32, 1, 0),
(5, 'https://www.youtube.com/watch?v=etQN4EfYN7k', 'Curso Java. Streams I. Accediend', 'Comenzamos en este vídeo el tema de los Streams (también llamados secuencias y flujo de datos)', 60, 'https://diylogodesigns.com/wp-content/uploads/2017/07/java-logo-vector-768x768.png', 'Video', 31, 5, 1),
(6, 'https://www.youtube.com/watch?v=XmUz5WJmJVU', 'Curso Java. POO I', 'En este vídeo comenzamos a ver en qué consiste la Programación orientada a Objetos', 99, 'https://diylogodesigns.com/wp-content/uploads/2017/07/java-logo-vector-768x768.png', 'Video', 36, 6, 1),
(7, 'https://www.youtube.com/watch?v=uUWEfmaFOkE', 'Curso Java. Programación genérica ArrayList I', 'Vemos en este vídeo la clase ArrayList. Con esta clase podremos almacenar en una lista dinámica objetos de diferentes tipos. Es ', 50, 'https://diylogodesigns.com/wp-content/uploads/2017/07/java-logo-vector-768x768.png', 'Video', 39, 5, 1),
(8, 'https://www.youtube.com/watch?v=wFzjvb0w-8Q', 'Curso Java. Programación genérica ArrayList II', 'Continuamos viendo los ArrayList. Vemos cómo acceder a un elemento en concreto dentro de un ArrayList y cómo copiar un ArrayList', 79, 'https://diylogodesigns.com/wp-content/uploads/2017/07/java-logo-vector-768x768.png', 'Video', 39, 7, 1),
(10, 'http://www4.tecnun.es/asignaturas/Informat1/AyudaInf/aprendainf/Java/Java2.pdf', 'Aprende java', 'Proyecto final de carrera de Javier García de Jalón donde enseña como programar en java desde 0', 50, '', 'Documento', 29, 10, 1),
(11, 'https://www.w3schools.com/java/java_user_input.asp', 'Scanner Java', 'Página de w3schools donde explica que es la clase scanner con ejemplos y ejercicios para realizar.', 50, 'https://diylogodesigns.com/wp-content/uploads/2017/07/java-logo-vector-768x768.png', 'Web', 32, 9, 1),
(15, 'https://www.youtube.com/watch?v=TKuxYHb-Hvc', 'Diagrama Entidad-Relación (ER) Parte 1', 'Aprende a crear un diagrama entidad-relación (ER) en este tutorial. Proporcionamos una descripción básica de los modelos ER y una formación paso a paso sobre cómo hacer un diagrama entidad-relación con la cardinalidad correcta.', 50, 'http://pluspng.com/img-png/youtube-png-youtube-transparent-png-image-512.png', 'Video', 2, 6, 1),
(16, 'https://www.youtube.com/watch?v=jshi9VCTm7g', 'Diagrama Entidad-Relación (ER) Parte 2', 'Aprende a crear un diagrama Entidad Relación con claves primarias, claves foráneas y claves compuestas con este tutorial avanzado.', 50, 'http://pluspng.com/img-png/youtube-png-youtube-transparent-png-image-512.png', 'Video', 2, 8, 1),
(17, 'https://www.youtube.com/watch?v=Z0yLerU0g-Q', 'Diagrama de Clases UML', 'Aprende cómo hacer clases, atributos y métodos en este tutorial de diagrama de clases UML. También repasamos en profundidad ejemplos sobre relaciones de herencia, agregación y composición.', 50, 'https://es.seaicons.com/wp-content/uploads/2015/11/MetroUI-YouTube-icon.png', 'Video', 16, 5, 1),
(18, 'https://elvex.ugr.es/idbis/db/docs/intro/D%20Modelo%20relacional.pdf', 'El modelo relacional', 'Documento Power Point donde explica y enseña como crear un modelo relacional.', 51, 'https://es.seaicons.com/wp-content/uploads/2015/11/document-file-icon.png', 'Documento', 3, 6, 1),
(19, 'https://www.w3schools.com/sql/sql_select.asp', 'Aprende a realizar consultas SQL', 'Página w3schools donde explica de forma teórica que es un SELECT y además tienes ejemplos para poder practicar.', 50, 'http://www.gpacero.es/wp-content/uploads/2017/06/Icono-WEB.png', 'Web', 7, 7, 1),
(20, 'https://www.google.es/url?sa=t&rct=j&q=&esrc=s&source=web&cd=3&cad=rja&uact=8&ved=2ahUKEwjrt8yV5NDiAhVDQhoKHQOiClQQFjACegQIAxAC&url=http%3A%2F%2Fwww.aulavirtual-exactas.dyndns.org%2Fclaroline%2Fbackends%2Fdownload.php%3Furl%3DL0dVSUEtVFAtMjAxNS9UNC1Nb2RMb2dpY28tTW9kUmVsYWMucGRm%26cidReset%3Dtrue%26cidReq%3DINBD_15AP&usg=AOvVaw0cAa04WvWPFyzjOzIGvxVD', 'DISEÑO LÓGICO: EL MODELO RELACIONAL', 'El diseño lógico es una descripción de los requisitos funcionales de un sistema. En otras palabras, es la expresión conceptual de lo que hará el sistema para resolver los problemas identificados en el análisis previo.', 50, 'https://www.zamzar.com/images/filetypes/doc.png', 'Documento', 4, 3, 1),
(21, 'https://es.wikiversity.org/wiki/Sistemas_de_almacenamiento', 'Sistemas de almacenamiento', 'Para almacenar estos datos se deben de elegir los sistemas de almacenamiento adecuados, dependiendo de tipo de datos y el volumen de estos.', 50, 'https://www.google.com/url?sa=i&rct=j&q=&esrc=s&source=images&cd=&ved=2ahUKEwjBtO765NDiAhXFyIUKHTMuABQQjRx6BAgBEAU&url=%2Furl%3Fsa%3Di%26rct%3Dj%26q%3D%26esrc%3Ds%26source%3Dimages%26cd%3D%26ved%3D2ahUKEwjBtO765NDiAhXFyIUKHTMuABQQjRx6BAgBEAU%26url%3Dhttps%253A%252F%252Fes.wikipedia.org%252Fwiki%252FWikipedia%26psig%3DAOvVaw050QekJFvjebb-npueCH_U%26ust%3D1559770741891351&psig=AOvVaw050QekJFvjebb-npueCH_U&ust=1559770741891351', 'Web', 1, 2, 1),
(22, 'https://es.wikipedia.org/wiki/Ingenier%C3%ADa_de_software', 'Ingeniería de software', 'La ingeniería de software es la aplicación de un enfoque sistemático, disciplinado y cuantificable al desarrollo, operación y mantenimiento de software,1? y el estudio de estos enfoques, es decir, el estudio de las aplicaciones de la ingeniería al software', 50, 'https://ugc.kn3.net/i/760x/http://turevista.files.wordpress.com/2008/06/installation-package.png', 'Web', 11, 8, 1),
(23, 'http://www.sitiolibre.com/curso/pdf/ED2.pdf', 'Instalación de Entornos Integrados de Desarrollo.', 'El uso de los entornos integrados de desarrollo se ratifica y afianza en los 90 y hoy en día contamos.', 50, 'https://es.seaicons.com/wp-content/uploads/2015/11/document-file-icon.png', 'Documento', 13, 3, 1),
(24, 'https://es.atlassian.com/continuous-delivery/software-testing/types-of-software-testing', 'The different types of software testing', 'There are many different types of testing that you can use to make sure that changes to your code are working as expected. Not all testing is equal, though, and we will see here how the main testing practices differ from each other.', 50, '', 'Web', 18, 2, 1),
(25, 'https://www.docencia.taboadaleon.es/1-1-lenguajes-de-marcas-tipos-y-clasificacion-de-los-mas-relevantes.html', 'Lenguajes de marcas: tipos y clasificación de los más relevantes', 'MÓDULO: LENGUAJES DE MARCAS Y SISTEMAS DE GESTIÓN DE LA INFORMACIÓN', 50, '', 'Web', 19, 11, 1),
(26, 'https://www.abrirllave.com/lmsgi/definicion-de-esquemas-y-vocabularios-en-xml.php', 'Definición de esquemas y vocabularios en XML', 'Web cona puntes, ejercicios y algunos recursos externos de interés.', 50, '', 'Web', 25, 10, 1),
(27, 'https://es.slideshare.net/emprendimientorfa/arquitectura-y-componentes-del-pc-13838465', 'Arquitectura y componentes del pc', 'Arquitectura y componentes del pc  Hardware: Parte física del Computador', 50, '', 'Web', 43, 8, 1),
(28, 'https://www.muycomputer.com/2018/07/25/administrador-de-discos-de-windows/', 'Guía de uso del administrador de discos de Windows', '6 tareas útiles del administrador de discos de Windows\r\nPublicado el 25 julio, 2018 por Juan Ranchal', 50, 'https://es.seaicons.com/wp-content/uploads/2015/10/browser-web-icon.png', 'Web', 47, 7, 1),
(29, 'https://www.youtube.com/watch?v=zJpr6eeugXo', 'Tratamiento y recuperación de datos XQUERY', 'Ejemplos de tyratamiento de datos en XQUERY explicados en un sencillo video.', 50, 'https://caudaldeportivo.es/wp-content/uploads/2014/12/1419812950_youtube-256.png', 'Video', 27, 4, 1),
(30, 'https://www.youtube.com/watch?v=idn4q-OWfws', 'Herramientas de mapeo objeto-relacional', 'Mapeo objeto-relacional con Hibernate', 51, 'https://caudaldeportivo.es/wp-content/uploads/2014/12/1419812950_youtube-256.png', 'Video', 53, 3, 1),
(31, 'http://ocw.udl.cat/enginyeria-i-arquitectura/programacio-2/continguts-1/4-manejo-bai81sico-de-archivos-en-java.pdf', 'Manejo básico de archivos en Java', 'Los programas usan variables para almacenar información: los datos de entrada, los resultados calculados y    valores intermedios generados a lo largo del cálculo.', 49, 'https://cdn0.iconfinder.com/data/icons/documents-26/32/documents-512.png', 'Documento', 51, 1, 1),
(32, 'https://www.tutorialspoint.com/es/xml/xml_databases.htm', 'XML - Bases de Datos', 'Base de datos XML se utiliza para almacenar la gran cantidad de información en formato XML. Como el uso de XML está aumentando en todos los campos, es necesario tener el lugar asegurado para almacenar los documentos XML. Los datos almacenados en la base de', 49, 'http://icons.iconarchive.com/icons/bokehlicia/captiva/256/browser-web-icon.png', 'Web', 55, 6, 1),
(33, 'https://www.campusmvp.es/recursos/post/que-es-la-plataforma-net-y-cuales-son-sus-principales-partes.aspx', 'Qué es la plataforma .NET y cuáles son sus principales partes', 'Simplificando mucho las cosas para poder dar una definición corta y comprensible, podríamos decir que la plataforma .NET es un amplio conjunto de bibliotecas de desarrollo que pueden ser utilizadas con el objetivo principal de acelerar el desarrollo de sof', 51, 'http://icons.iconarchive.com/icons/bokehlicia/captiva/256/browser-web-icon.png', 'Web', 56, 5, 1),
(34, 'https://visualstudio.microsoft.com/es/', 'Visual Studio para PC y Mac', 'Escribir código con rapidez\r\nDepurar y emitir diagnósticos fácilmente\r\nRealizar pruebas periódicas y publicar con confianza\r\nExtender y personalizar según sus preferencias\r\nColaborar de manera eficiente', 49, 'https://img.icons8.com/color/420/visual-studio.png', 'Web', 57, 3, 1),
(35, 'https://developer.microsoft.com/es-es/windows/apps', 'Desarrollar aplicaciones para UWP', 'Usa las funciones más avanzadas para crear aplicaciones que resulten personales, naturales e intuitivas.', 51, '', 'Web', 61, 11, 1),
(36, 'https://www.youtube.com/watch?v=Jt0-1-RigIw', 'Ado.NET - Parte 1 - Introducción', 'Tutoriales introductorios para el uso de esta tecnología en la creación de aplicaciones en C# Información entregada a través de la página de Microsoft para ADO.NET:', 49, 'https://caudaldeportivo.es/wp-content/uploads/2014/12/1419812950_youtube-256.png', 'Video', 63, 2, 1),
(37, 'https://www.youtube.com/watch?v=gabehBpbX9o&list=PLF3O845vu6lDXQncYYTo8DoJ7nPruADFD', 'Sistemas de Gestión Empresarial - Instalación de Odoo 10', 'Instalación de Odoo 10 directamente de los repositorios en Ubuntu 16.04.', 51, 'https://caudaldeportivo.es/wp-content/uploads/2014/12/1419812950_youtube-256.png', 'Video', 67, 7, 1),
(38, 'https://www.uv.es/catedra-aeca/workshop/files/files/SP2_Andres_Lorca.pdf', 'IMPLANTACIÓN DE LOS SISTEMAS ERP POR LAS EMPRESAS ESPAÑOLAS', 'El presente trabajo analiza la incidencia de la implantación de los sistemas ERP (Enterprise Resource Planning)  sobre  un  conjunto  de  variables  económico-financieras.', 51, 'https://www.freeiconspng.com/uploads/circle-document-documents-extension-file-page-sheet-icon-7.png', 'Documento', 69, 10, 1),
(39, 'https://www.youtube.com/watch?v=qXhc4wbDaqU', 'Curso Java. Threads I. Programación de hilos.', 'En este vídeo comenzamos a ver la programación de Threads o hilos. Esto nos permitirá crear programas de Java que sean capaces de ejecutar varias tareas de forma simultánea.', 49, 'https://caudaldeportivo.es/wp-content/uploads/2014/12/1419812950_youtube-256.png', 'Video', 72, 9, 1),
(40, 'https://w3.ual.es/~rguirado/so/tema2.pdf', 'PROCESOS E HILOS: CONCURRENCIA, SINCRONIZACIÓN Y COMUNICACIÓN', 'IntroducciónDefinición de proceso (tarea según los fabricantes):\r\nPrograma en ejecución\r\nCálculo computacional que puede hacerse concurrentemente con otros cálculos\r\nSecuencia  de  acciones llevadas a cabo a través de la ejecución', 51, 'http://icons.iconarchive.com/icons/pelfusion/flat-file-type/256/doc-icon.png', 'Documento', 74, 5, 1),
(41, 'https://oscarmaestre.github.io/servicios/textos/tema3.html', 'Programación de comunicaciones en red', 'En Java toda la comunicación vista en primer curso de DAM consiste en dos cosas\r\nEntrada/salida por consola: con las clases System.in o System.out.\r\nLectura/escritura en ficheros: con las clases File y similares.\r\nSe puede avanzar un paso más u', 49, 'http://icons.iconarchive.com/icons/bokehlicia/captiva/256/browser-web-icon.png', 'Web', 75, 2, 1),
(42, 'https://www.google.es/url?sa=t&rct=j&q=&esrc=s&source=web&cd=1&ved=2ahUKEwiqr-ChyNLiAhUjx4UKHUgMA30QFjAAegQIABAC&url=http%3A%2F%2Fwww.ra-ma.es%2Fdescargas%2Fdescargar.php%3Ffichero%3DZ3dkZXNjYXJnYXNwcm9mIzI2MSM5Nzg4NDk5NjQxNzA2X0NhcGl0dWxvIDEucGRm&usg=AOvVaw16bnwb8BcCnqDr-76m1f2W', 'Análisis de tecnologías para aplicaciones en dispositivos móvile', 'Para trabajar con los distintos entornos de desarrollo de aplicaciones para dispositivos móviles. Análisis de tecnologías para aplicaciones en dispositivos...', 50, 'http://icons.iconarchive.com/icons/pelfusion/flat-file-type/256/doc-icon.png', 'Documento', 78, 3, 1),
(43, 'https://www.youtube.com/watch?v=Y_PKWr_1rDw', 'Curso Unity 5: Juego de Plataformas 2D [#1] Tiles y Escenario', 'NUEVA WEB: https://www.hektorprofe.net/ RECURSOS UNITY: https://www.hektorprofe.net/escuelade... RECURSOS GAMEMAKER: https://www.hektorprofe.net/escuelade... Todo lo que he hecho lo encontraréis en los 3 enlaces de arriba, buscad en los apartados pertinent', 50, 'https://caudaldeportivo.es/wp-content/uploads/2014/12/1419812950_youtube-256.png', 'Video', 86, 11, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `notas`
--

DROP TABLE IF EXISTS `notas`;
CREATE TABLE `notas` (
  `Id` int(5) NOT NULL,
  `Usuario` int(5) NOT NULL,
  `Asignatura` int(1) NOT NULL,
  `Trimestre` int(1) NOT NULL,
  `Nota` decimal(4,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Volcado de datos para la tabla `temas`
--

INSERT INTO `notas` (`Id`, `Usuario`, `Asignatura`, `Trimestre`, `Nota`) VALUES 
(1, 2, 1, 1, 9.94),
(2, 2, 1, 2, 4.65),
(3, 2, 1, 3, 7.65),
(4, 2, 2, 1, 9.96),
(5, 2, 2, 2, 8.48),
(6, 2, 2, 3, 1.47),
(7, 2, 3, 1, 7.71),
(8, 2, 3, 2, 9.29),
(9, 2, 3, 3, 3.54),
(10, 2, 4, 1, 8.66),
(11, 2, 4, 2, 8.58),
(12, 2, 4, 3, 5.83),
(13, 2, 5, 1, 6.85),
(14, 2, 5, 2, 6.29),
(15, 2, 5, 3, 8.35),
(16, 2, 6, 1, 7.42),
(17, 2, 6, 2, 5.35),
(18, 2, 7, 1, 8.25),
(19, 2, 7, 2, 1.59),
(20, 2, 8, 1, 3.04),
(21, 2, 8, 2, 3.39),
(22, 2, 9, 1, 2.21),
(23, 2, 9, 2, 0.00),
(24, 2, 10, 1, 3.92),
(25, 2, 10, 2, 1.15),
(26, 2, 11, 1, 9.24),
(27, 2, 11, 2, 6.63),
(28, 2, 11, 3, 1.61),
(29, 2, 12, 1, 3.73),
(30, 2, 12, 2, 4.77),
(31, 2, 13, 1, 9.58),
(32, 2, 13, 2, 8.22),
(33, 3, 1, 1, 8.63),
(34, 3, 1, 2, 4.43),
(35, 3, 1, 3, 5.28),
(36, 3, 2, 1, 0.00),
(37, 3, 2, 2, 9.16),
(38, 3, 2, 3, 6.14),
(39, 3, 3, 1, 2.58),
(40, 3, 3, 2, 7.43),
(41, 3, 3, 3, 8.89),
(42, 3, 4, 1, 6.01),
(43, 3, 4, 2, 6.32),
(44, 3, 4, 3, 3.71),
(45, 3, 5, 1, 2.06),
(46, 3, 5, 2, 9.14),
(47, 3, 5, 3, 0.00),
(48, 3, 6, 1, 6.88),
(49, 3, 6, 2, 8.22),
(50, 3, 7, 1, 6.06),
(51, 3, 7, 2, 9.84),
(52, 3, 8, 1, 6.80),
(53, 3, 8, 2, 7.69),
(54, 3, 9, 1, 3.59),
(55, 3, 9, 2, 9.81),
(56, 3, 10, 1, 1.54),
(57, 3, 10, 2, 5.59),
(58, 3, 11, 1, 6.49),
(59, 3, 11, 2, 1.14),
(60, 3, 11, 3, 9.33),
(61, 3, 12, 1, 8.53),
(62, 3, 12, 2, 9.25),
(63, 3, 13, 1, 2.95),
(64, 3, 13, 2, 3.39),
(65, 4, 1, 1, 3.42),
(66, 4, 1, 2, 0.00),
(67, 4, 1, 3, 0.00),
(68, 4, 2, 1, 5.39),
(69, 4, 2, 2, 3.12),
(70, 4, 2, 3, 5.98),
(71, 4, 3, 1, 8.07),
(72, 4, 3, 2, 8.80),
(73, 4, 3, 3, 3.34),
(74, 4, 4, 1, 9.32),
(75, 4, 4, 2, 5.43),
(76, 4, 4, 3, 1.35),
(77, 4, 5, 1, 8.08),
(78, 4, 5, 2, 6.07),
(79, 4, 5, 3, 8.38),
(80, 4, 6, 1, 0.00),
(81, 4, 6, 2, 3.13),
(82, 4, 7, 1, 6.59),
(83, 4, 7, 2, 5.96),
(84, 4, 8, 1, 1.57),
(85, 4, 8, 2, 2.74),
(86, 4, 9, 1, 3.56),
(87, 4, 9, 2, 1.66),
(88, 4, 10, 1, 8.79),
(89, 4, 10, 2, 4.08),
(90, 4, 11, 1, 4.96),
(91, 4, 11, 2, 4.80),
(92, 4, 11, 3, 7.77),
(93, 4, 12, 1, 6.65),
(94, 4, 12, 2, 0.00),
(95, 4, 13, 1, 9.08),
(96, 4, 13, 2, 2.96),
(97, 6, 1, 1, 4.35),
(98, 6, 1, 2, 8.87),
(99, 6, 1, 3, 6.48),
(100, 6, 2, 1, 4.89),
(101, 6, 2, 2, 5.73),
(102, 6, 2, 3, 0.00),
(103, 6, 3, 1, 3.66),
(104, 6, 3, 2, 5.96),
(105, 6, 3, 3, 1.18),
(106, 6, 4, 1, 7.17),
(107, 6, 4, 2, 6.11),
(108, 6, 4, 3, 7.84),
(109, 6, 5, 1, 3.07),
(110, 6, 5, 2, 0.00),
(111, 6, 5, 3, 3.41),
(112, 6, 6, 1, 7.84),
(113, 6, 6, 2, 8.98),
(114, 6, 7, 1, 6.29),
(115, 6, 7, 2, 9.90),
(116, 6, 8, 1, 2.62),
(117, 6, 8, 2, 4.38),
(118, 6, 9, 1, 4.48),
(119, 6, 9, 2, 9.39),
(120, 6, 10, 1, 1.59),
(121, 6, 10, 2, 3.59),
(122, 6, 11, 1, 2.55),
(123, 6, 11, 2, 2.60),
(124, 6, 11, 3, 4.59),
(125, 6, 12, 1, 2.74),
(126, 6, 12, 2, 4.60),
(127, 6, 13, 1, 2.18),
(128, 6, 13, 2, 4.24),
(129, 7, 1, 1, 2.39),
(130, 7, 1, 2, 1.20),
(131, 7, 1, 3, 4.47),
(132, 7, 2, 1, 8.99),
(133, 7, 2, 2, 0.00),
(134, 7, 2, 3, 6.20),
(135, 7, 3, 1, 5.49),
(136, 7, 3, 2, 9.35),
(137, 7, 3, 3, 0.00),
(138, 7, 4, 1, 2.85),
(139, 7, 4, 2, 2.59),
(140, 7, 4, 3, 6.94),
(141, 7, 5, 1, 5.27),
(142, 7, 5, 2, 7.93),
(143, 7, 5, 3, 3.05),
(144, 7, 6, 1, 9.13),
(145, 7, 6, 2, 6.29),
(146, 7, 7, 1, 6.20),
(147, 7, 7, 2, 2.75),
(148, 7, 8, 1, 8.45),
(149, 7, 8, 2, 8.39),
(150, 7, 9, 1, 9.87),
(151, 7, 9, 2, 9.81),
(152, 7, 10, 1, 2.05),
(153, 7, 10, 2, 8.29),
(154, 7, 11, 1, 0.00),
(155, 7, 11, 2, 9.23),
(156, 7, 11, 3, 7.24),
(157, 7, 12, 1, 6.95),
(158, 7, 12, 2, 7.17),
(159, 7, 13, 1, 7.20),
(160, 7, 13, 2, 2.67),
(161, 9, 1, 1, 3.39),
(162, 9, 1, 2, 0.00),
(163, 9, 1, 3, 3.11),
(164, 9, 2, 1, 4.43),
(165, 9, 2, 2, 4.34),
(166, 9, 2, 3, 8.52),
(167, 9, 3, 1, 2.93),
(168, 9, 3, 2, 9.98),
(169, 9, 3, 3, 8.85),
(170, 9, 4, 1, 0.00),
(171, 9, 4, 2, 0.00),
(172, 9, 4, 3, 3.38),
(173, 9, 5, 1, 4.29),
(174, 9, 5, 2, 1.35),
(175, 9, 5, 3, 1.68),
(176, 9, 6, 1, 9.57),
(177, 9, 6, 2, 8.32),
(178, 9, 7, 1, 2.40),
(179, 9, 7, 2, 8.13),
(180, 9, 8, 1, 7.13),
(181, 9, 8, 2, 3.38),
(182, 9, 9, 1, 3.93),
(183, 9, 9, 2, 7.53),
(184, 9, 10, 1, 6.36),
(185, 9, 10, 2, 1.21),
(186, 9, 11, 1, 1.78),
(187, 9, 11, 2, 6.93),
(188, 9, 11, 3, 7.35),
(189, 9, 12, 1, 3.29),
(190, 9, 12, 2, 4.61),
(191, 9, 13, 1, 2.03),
(192, 9, 13, 2, 5.78),
(193, 11, 1, 1, 1.33),
(194, 11, 1, 2, 3.02),
(195, 11, 1, 3, 5.34),
(196, 11, 2, 1, 9.78),
(197, 11, 2, 2, 1.58),
(198, 11, 2, 3, 4.84),
(199, 11, 3, 1, 5.32),
(200, 11, 3, 2, 7.48),
(201, 11, 3, 3, 7.68),
(202, 11, 4, 1, 6.54),
(203, 11, 4, 2, 3.17),
(204, 11, 4, 3, 8.38),
(205, 11, 5, 1, 2.75),
(206, 11, 5, 2, 6.90),
(207, 11, 5, 3, 6.58),
(208, 11, 6, 1, 6.14),
(209, 11, 6, 2, 8.36),
(210, 11, 7, 1, 5.74),
(211, 11, 7, 2, 4.77),
(212, 11, 8, 1, 2.90),
(213, 11, 8, 2, 3.90),
(214, 11, 9, 1, 0.00),
(215, 11, 9, 2, 1.86),
(216, 11, 10, 1, 4.90),
(217, 11, 10, 2, 9.10),
(218, 11, 11, 1, 3.10),
(219, 11, 11, 2, 2.69),
(220, 11, 11, 3, 3.42),
(221, 11, 12, 1, 1.39),
(222, 11, 12, 2, 6.99),
(223, 11, 13, 1, 1.97),
(224, 11, 13, 2, 0.00);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `temas`
--

DROP TABLE IF EXISTS `temas`;
CREATE TABLE `temas` (
  `Id` int(2) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Asignatura` int(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Volcado de datos para la tabla `temas`
--

INSERT INTO `temas` (`Id`, `Nombre`, `Asignatura`) VALUES
(1, 'Tema 01. Sistemas de almacenamiento de información', 1),
(2, 'Tema 02. El modelo entidad/interrelación', 1),
(3, 'Tema 03. El modelo relacional', 1),
(4, 'Tema 04. Diseño lógico en el modelo relacional', 1),
(5, 'Tema 05. Lenguaje DDL. Creación y gestión de tablas', 1),
(6, 'Tema 06. Gestión de la seguridad', 1),
(7, 'Tema 07. Realización de consultas', 1),
(8, 'Tema 08. El Lenguaje SQL como DML para Edición de los Datos', 1),
(9, 'Tema 09. Programación de Bases de Datos', 1),
(10, 'Tema 01. El Programa informático', 2),
(11, 'Tema 02. La Ingeniería de software', 2),
(12, 'Tema 03. Del código fuente al ejecutable', 2),
(13, 'Tema 04. Entornos de desarrollo: Instalación y uso', 2),
(14, 'Tema 05. Gestión de la configuración', 2),
(15, 'Tema 06. Introducción al UML', 2),
(16, 'Tema 07. UML y los diagramas de clases', 2),
(17, 'Tema 08. UML y los diagramas de comportamiento', 2),
(18, 'Tema 09. Pruebas de software', 2),
(19, 'Tema 01. Lenguajes de marcas', 3),
(20, 'Tema 02. Lenguajes para la visualización de la información I', 3),
(21, 'Tema 03. Hojas de estilo en cascada I', 3),
(22, 'Tema 04. Lenguajes para la visualización de la información II', 3),
(23, 'Tema 05. Hojas de estilo en cascada II', 3),
(24, 'Tema 06. XML', 3),
(25, 'Tema 07. Definición de esquemas en y vocabularios en XML', 3),
(26, 'Tema 08. Conversión y adaptación de documentos XML', 3),
(27, 'Tema 09. Tratamiento y recuperación de datos XQUERY', 3),
(28, 'Tema 10. Sindicación de contenidos mediante lenguajes de marcas', 3),
(29, 'Tema 01. Introducción al lenguaje JAVA - Entorno de programación', 4),
(30, 'Tema 02. Elementos de un programa', 4),
(31, 'Tema 03. Lectura y escritura de información - Streams', 4),
(32, 'Tema 04. Lectura y escritura de información - Scanner', 4),
(33, 'Tema 05. Estructuras de control de flujo', 4),
(34, 'Tema 06. Introduccion a la programación orientada a objetos', 4),
(35, 'Tema 07. Estructuras de datos', 4),
(36, 'Tema 08. Desarrollo de clases e instanciación de objetos', 4),
(37, 'Tema 09. Control y manejo de excepciones', 4),
(38, 'Tema 10. Utilizacion avanzada de clases', 4),
(39, 'Tema 11. Colecciones de datos - Listas, pilas y colas', 4),
(40, 'Tema 12. Estructuras externas de datos - Ficheros', 4),
(41, 'Tema 13. Interfaces graficas de usuario', 4),
(42, 'Tema 01. Representacion de la informacion', 5),
(43, 'Tema 02. Arquitectura y componentes del pc', 5),
(44, 'Tema 03. Introduccion a los sistemas operativos', 5),
(45, 'Tema 04. Administracion de sistemas operativos propietarios', 5),
(46, 'Tema 05. Sistemas de archivos en sistemas operativos propietarios', 5),
(47, 'Tema 06. Administracion de discos', 5),
(48, 'Tema 07. Sistemas operativos libres: ubuntu sistema de archivos', 5),
(49, 'Tema 08. Redes cisco', 5),
(50, 'Tema 09. Directorio activo', 5),
(51, 'Tema 01. Manejo de ficheros', 6),
(52, 'Tema 02. Manejo de colectores', 6),
(53, 'Tema 03. Herramientas de mapeo objeto-relacional', 6),
(54, 'Tema 04. BBDD objeto-relacionales y orientadas a objetos', 6),
(55, 'Tema 05. Bases de datos XML', 6),
(56, 'Tema 01. La plataforma .NET', 7),
(57, 'Tema 02. Herramienta VisualStudio', 7),
(58, 'Tema 03. La organización de una aplicación', 7),
(59, 'Tema 04. Las bases del lenguaje', 7),
(60, 'Tema 05. Programación orientada a objetos', 7),
(61, 'Tema 06. Desarrollo de aplicaciones en Windows', 7),
(62, 'Tema 07. Depuración y gestión de errores', 7),
(63, 'Tema 08. ADO.NET', 7),
(64, 'Tema 09. LINQ', 7),
(65, 'Tema 10. Aplicaciones WPF', 7),
(66, 'Tema 01. Introducción a los Sistemas de Gestión Empresarial', 8),
(67, 'Tema 02. Instalación y configuración de sistemas ERP-CRM', 8),
(68, 'Tema 03. Organización y consulta de la información', 8),
(69, 'Tema 04. Implantación de sistemas ERP-CRM en una empresa', 8),
(70, 'Tema 05. Desarrollo de componentes', 8),
(71, 'Tema 01. Los procesos en unix', 9),
(72, 'Tema 02. Programación multiproceso', 9),
(73, 'Tema 03. Los hilos de ejecución', 9),
(74, 'Tema 04. Comunicación y sincronización entre hilos', 9),
(75, 'Tema 05. Programación de comunicaciones en red', 9),
(76, 'Tema 06. Servicios en red', 9),
(77, 'Tema 07. Programación segura', 9),
(78, 'Tema 01. Análisis y tecnologías para dispositivos móviles', 10),
(79, 'Tema 02. Dispositivos móviles - Actividades', 10),
(80, 'Tema 03. Dispositivos móviles - Interfaz de usuario', 10),
(81, 'Tema 04. Dispositivos móviles - Mensajería y comunicación', 10),
(82, 'Tema 05. Dispositivos móviles - Almacenamiento', 10),
(83, 'Tema 06. Dispositivos móviles - Bases de datos y listas', 10),
(84, 'Tema 07. Dispositivos móviles - Integración de librerías multimedia', 10),
(85, 'Tema 08. Dispositivos móviles - Animaciones y juegos', 10),
(86, 'Tema 09. Motor gráfico Unity - Desarrollo de juegos 2D/3D', 10);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

DROP TABLE IF EXISTS `usuarios`;
CREATE TABLE `usuarios` (
  `Id` int(5) NOT NULL,
  `Nombre` varchar(32) NOT NULL,
  `Curso` int(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`Id`, `Nombre`, `Curso`) VALUES
(1, 'admin', 0),
(2, 'antonio', 2),
(3, 'ruben', 2),
(4, 'valentin', 2),
(5, 'ana', 0),
(6, 'maria', 1),
(7, 'juan', 0),
(8, 'susana', 0),
(9, 'carlos', 1),
(10, 'jessica', 0),
(11, 'pablo', 1);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `asignaturas`
--
ALTER TABLE `asignaturas`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `Nombre` (`Nombre`),
  ADD KEY `fk_curso_id2` (`Curso`);

--
-- Indices de la tabla `curso`
--
ALTER TABLE `curso`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `enlaces`
--
ALTER TABLE `enlaces`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `Link` (`Link`),
  ADD KEY `fk_tema_id` (`Tema`),
  ADD KEY `fk_uploader_id` (`Uploader`);

--
-- Indices de la tabla `notas`
--
ALTER TABLE `notas`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `fk_usuario_id` (`Usuario`),
  ADD KEY `fk_asignatura_id` (`Asignatura`);

--
-- Indices de la tabla `temas`
--
ALTER TABLE `temas`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `Nombre` (`Nombre`),
  ADD KEY `fk_asignatura_id2` (`Asignatura`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `Nombre` (`Nombre`),
  ADD KEY `fk_curso_id` (`Curso`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `asignaturas`
--
ALTER TABLE `asignaturas`
  MODIFY `Id` int(1) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT de la tabla `enlaces`
--
ALTER TABLE `enlaces`
  MODIFY `Id` int(3) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=44;

--
-- AUTO_INCREMENT de la tabla `notas`
--
ALTER TABLE `notas`
  MODIFY `Id` int(5) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `temas`
--
ALTER TABLE `temas`
  MODIFY `Id` int(2) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=87;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `Id` int(5) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `asignaturas`
--
ALTER TABLE `asignaturas`
  ADD CONSTRAINT `fk_curso_id2` FOREIGN KEY (`Curso`) REFERENCES `curso` (`Id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `enlaces`
--
ALTER TABLE `enlaces`
  ADD CONSTRAINT `fk_tema_id` FOREIGN KEY (`Tema`) REFERENCES `temas` (`Id`),
  ADD CONSTRAINT `fk_uploader_id` FOREIGN KEY (`Uploader`) REFERENCES `usuarios` (`Id`);

--
-- Filtros para la tabla `notas`
--
ALTER TABLE `notas`
  ADD CONSTRAINT `fk_asignatura_id` FOREIGN KEY (`Asignatura`) REFERENCES `asignaturas` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `fk_usuario_id` FOREIGN KEY (`Usuario`) REFERENCES `usuarios` (`Id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `temas`
--
ALTER TABLE `temas`
  ADD CONSTRAINT `fk_asignatura_id2` FOREIGN KEY (`Asignatura`) REFERENCES `asignaturas` (`Id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD CONSTRAINT `fk_curso_id` FOREIGN KEY (`Curso`) REFERENCES `curso` (`Id`) ON DELETE CASCADE;
COMMIT;





/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

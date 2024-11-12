-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 12-11-2024 a las 02:41:03
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `sams_inmobiliaria`
--
CREATE DATABASE IF NOT EXISTS `sams_inmobiliaria` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `sams_inmobiliaria`;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contrato`
--

CREATE TABLE `contrato` (
  `id` int(11) NOT NULL,
  `id_inquilino` int(11) NOT NULL,
  `id_inmueble` int(11) NOT NULL,
  `fecha_inicio` date DEFAULT NULL,
  `fecha_final` date DEFAULT NULL,
  `fecha_correcta` date DEFAULT NULL,
  `monto` decimal(10,5) DEFAULT NULL,
  `borrado` tinyint(1) DEFAULT 0,
  `fecha_creado` datetime NOT NULL DEFAULT current_timestamp(),
  `fecha_cancelado` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`id`, `id_inquilino`, `id_inmueble`, `fecha_inicio`, `fecha_final`, `fecha_correcta`, `monto`, `borrado`, `fecha_creado`, `fecha_cancelado`) VALUES
(3, 5, 1, '2024-10-01', '2027-10-01', '2027-10-01', 99999.99999, 1, '2024-10-26 00:23:42', NULL),
(17, 1, 4, '2024-10-01', '2025-10-01', '2025-10-01', 4000.00000, 1, '2024-10-26 00:44:44', NULL),
(18, 2, 2, '2024-10-05', '2025-10-05', '2025-10-05', 99999.99999, 1, '2024-10-26 00:45:00', NULL),
(19, 3, 3, '2024-10-10', '2025-10-10', '2025-10-10', 99999.99999, 1, '2024-10-26 00:45:16', NULL),
(20, 4, 4, '2020-01-01', '2023-01-01', '2023-01-01', 99999.99999, 1, '2024-10-26 00:45:29', NULL),
(21, 5, 5, '2021-05-01', '2023-05-01', '2023-05-01', 99999.99999, 0, '2024-10-26 00:45:43', NULL),
(22, 2, 1, '2023-01-01', '2024-01-01', '2024-01-01', 99999.99999, 1, '2024-10-26 00:45:54', '2024-01-10 00:00:00');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmueble`
--

CREATE TABLE `inmueble` (
  `id` int(11) NOT NULL,
  `direccion` varchar(250) NOT NULL,
  `coordenadas` varchar(250) DEFAULT NULL,
  `uso` enum('comercial','residencial') NOT NULL DEFAULT 'comercial',
  `id_tipo` int(11) NOT NULL,
  `cant_ambientes` tinyint(2) NOT NULL DEFAULT 1,
  `descripcion` text DEFAULT NULL,
  `avatarUrl` varchar(250) DEFAULT NULL,
  `precio` decimal(12,5) DEFAULT NULL,
  `id_propietario` int(11) NOT NULL,
  `estado` enum('disponible','retirado') DEFAULT 'retirado',
  `borrado` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`id`, `direccion`, `coordenadas`, `uso`, `id_tipo`, `cant_ambientes`, `descripcion`, `avatarUrl`, `precio`, `id_propietario`, `estado`, `borrado`) VALUES
(1, 'Lavalle 115', 'Concordia y Laprida', 'residencial', 1, 4, 'Casa con antiguedad de 10 años, todos los servicios, premium', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRGWtAjj1Y0n4q-WoWLLIxyrzH2KM8vJfToPw&s', 500000.00000, 4, 'retirado', 0),
(2, 'Av. corrientes', ' peatonal San Martin', 'residencial', 2, 3, 'ambiente espacioso, luminoso, con todos los servicios', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTBjcfSDEtEHBRZEgtyszF0H4OhwTwJz6vg5A&s', 300000.00000, 4, 'disponible', 0),
(3, 'Av. Serrana', 'Juana Koslay', 'residencial', 3, 6, 'Casa quinta ideal para reuniones, festejos, todos los servicios, equipada, con piscina', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSByMzCMYxvjlFhm4hePOQAKR-f0SDCSbggug&s', 300000.00000, 4, 'retirado', 0),
(4, 'Los colibries', 'Ministro Ponce', 'comercial', 4, 2, 'Local comercial remodelado , todos los servicios, buena ubicacion y clientela', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQtoCFVfDrPOxk_m1Y5v0uzmXSYwUEa-298gA&s', 250000.00000, 4, 'disponible', 0),
(5, 'Av. Avellaneda', 'Juan B justo', 'comercial', 2, 4, 'Ideal para familia tipo, todos los servicios, muy confort', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ9kiW8jkr5TjDhEcruff_qS3Q1xVcWBfB7dQ&s', 320000.00000, 4, 'retirado', 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilino`
--

CREATE TABLE `inquilino` (
  `id` int(11) NOT NULL,
  `dni` varchar(60) NOT NULL,
  `apellido` varchar(60) NOT NULL,
  `nombre` varchar(60) NOT NULL,
  `telefono` varchar(50) NOT NULL,
  `email` varchar(100) NOT NULL,
  `borrado` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilino`
--

INSERT INTO `inquilino` (`id`, `dni`, `apellido`, `nombre`, `telefono`, `email`, `borrado`) VALUES
(1, '272944562', 'Pericles', 'Pedro', '2667933', 'peri@gmail.com', 0),
(2, '123456789', 'García', 'María', '2661234567', 'mgarcia@gmail.com', 0),
(3, '987654321', 'López', 'Carlos', '2667654321', 'clopez@yahoo.com', 0),
(4, '456789123', 'Martínez', 'Lucía', '2665432109', 'lmartinez@hotmail.com', 0),
(5, '321654987', 'Hernández', 'Fernando', '2669876543', 'fhernandez@outlook.com', 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

CREATE TABLE `pago` (
  `id` int(11) NOT NULL,
  `id_contrato` int(11) NOT NULL,
  `importe` decimal(12,5) NOT NULL,
  `fecha_pago` date NOT NULL DEFAULT current_timestamp(),
  `cod_pago` int(11) NOT NULL DEFAULT 1,
  `detalle` varchar(200) NOT NULL,
  `estado` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pago`
--

INSERT INTO `pago` (`id`, `id_contrato`, `importe`, `fecha_pago`, `cod_pago`, `detalle`, `estado`) VALUES
(1, 3, 500000.00000, '2024-10-26', 101, 'pago en termino', 1),
(2, 17, 600000.00000, '2024-10-26', 102, 'fuera de termino', 1),
(3, 18, 600000.00000, '2024-09-10', 103, 'en termino', 0),
(4, 19, 400000.00000, '2024-09-01', 104, 'fuera de termino', 1),
(5, 20, 500000.00000, '2024-10-14', 105, 'en termino', 1),
(6, 21, 250000.00000, '2024-10-22', 106, 'pago mes deposito', 0),
(7, 22, 500000.00000, '2024-07-02', 107, 'termino', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietario`
--

CREATE TABLE `propietario` (
  `id` int(11) NOT NULL,
  `dni` varchar(50) NOT NULL,
  `apellido` varchar(60) NOT NULL,
  `nombre` varchar(60) NOT NULL,
  `telefono` varchar(50) NOT NULL,
  `email` varchar(250) NOT NULL,
  `password` varchar(250) DEFAULT NULL,
  `avatarUrl` varchar(200) DEFAULT NULL,
  `borrado` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietario`
--

INSERT INTO `propietario` (`id`, `dni`, `apellido`, `nombre`, `telefono`, `email`, `password`, `avatarUrl`, `borrado`) VALUES
(2, '453333', 'escudero', 'soledad', '2334343', 'marusofok2@gmail.com', '$2a$12$mj716Ytz44lE4f1n1rBnOOXKWGNlxwWDYpNiCFycbOw2so8XJEWte', NULL, 0),
(4, '45456567', 'Kim', 'Hyun Soo', '2664999', 'juan@gmail.com', '$2a$11$BeD58Y4V2BkoF/mVXDp4/OrGSIpdcgf3/omp2hjC0oHpNBSv2ZM2.', 'https://www.infobae.com/resizer/v2/GUIIGIVINVGTLEOIDDRCLEXY54.jpg?auth=d14f118b8294ca7d441b362880f0e45633fc096324cd95df0727eebba49b19d1&smart=true&width=992&height=558&quality=85', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipo_inmueble`
--

CREATE TABLE `tipo_inmueble` (
  `id` int(11) NOT NULL,
  `tipo` varchar(250) NOT NULL,
  `borrado` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `tipo_inmueble`
--

INSERT INTO `tipo_inmueble` (`id`, `tipo`, `borrado`) VALUES
(1, 'casa', 0),
(2, 'departamento', 0),
(3, 'casa quinta', 0),
(4, 'local', 0),
(5, 'duplex', 0),
(6, 'campo', 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `id` int(11) NOT NULL,
  `dni` varchar(50) NOT NULL,
  `apellido` varchar(60) NOT NULL,
  `nombre` varchar(60) NOT NULL,
  `email` varchar(60) NOT NULL,
  `password` varchar(250) NOT NULL,
  `rol` enum('usuario','administrador') NOT NULL DEFAULT 'usuario',
  `avatarUrl` varchar(250) DEFAULT NULL,
  `borrado` tinyint(1) NOT NULL DEFAULT 0,
  `update_at` datetime NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`id`, `dni`, `apellido`, `nombre`, `email`, `password`, `rol`, `avatarUrl`, `borrado`, `update_at`) VALUES
(1, '30283944', 'Miranda', 'Pedro', 'kimsoo@gmail.com', 'pepe2014', 'usuario', '', 0, '2024-10-26 03:22:19'),
(2, '453333', 'Escudero', 'soledad', 'marusofok2@gmail.com', 'samu2l19', 'usuario', NULL, 1, '2024-10-21 20:56:02'),
(3, '45456456', 'Hyun', 'Kim soo', 'juan@gmail.com', '$2a$12$d6u2uwMNWRH.DdkZIcQS2eWjvnHEUyRbHP58ZBl2V0L7PZCWJy2VK', 'usuario', 'https://images.freeimages.com/365/images/istock/previews/9730/97305669-avatar-icon-of-girl-in-a-baseball-cap.jpg', 1, '2024-10-26 03:22:54');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `fecha_inicio` (`fecha_inicio`,`fecha_final`),
  ADD KEY `id_inquilino` (`id_inquilino`),
  ADD KEY `id_inmueble` (`id_inmueble`);

--
-- Indices de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD PRIMARY KEY (`id`),
  ADD KEY `id_tipo` (`id_tipo`),
  ADD KEY `id_propietario` (`id_propietario`);

--
-- Indices de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `email` (`email`);

--
-- Indices de la tabla `pago`
--
ALTER TABLE `pago`
  ADD PRIMARY KEY (`id`),
  ADD KEY `id_contrato` (`id_contrato`);

--
-- Indices de la tabla `propietario`
--
ALTER TABLE `propietario`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `email` (`email`);

--
-- Indices de la tabla `tipo_inmueble`
--
ALTER TABLE `tipo_inmueble`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `tipo` (`tipo`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contrato`
--
ALTER TABLE `contrato`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=23;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `propietario`
--
ALTER TABLE `propietario`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `tipo_inmueble`
--
ALTER TABLE `tipo_inmueble`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD CONSTRAINT `contrato_ibfk_1` FOREIGN KEY (`id_inquilino`) REFERENCES `inquilino` (`id`),
  ADD CONSTRAINT `contrato_ibfk_2` FOREIGN KEY (`id_inmueble`) REFERENCES `inmueble` (`id`);

--
-- Filtros para la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD CONSTRAINT `inmueble_ibfk_1` FOREIGN KEY (`id_tipo`) REFERENCES `tipo_inmueble` (`id`),
  ADD CONSTRAINT `inmueble_ibfk_2` FOREIGN KEY (`id_propietario`) REFERENCES `propietario` (`id`);

--
-- Filtros para la tabla `pago`
--
ALTER TABLE `pago`
  ADD CONSTRAINT `pago_ibfk_1` FOREIGN KEY (`id_contrato`) REFERENCES `contrato` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

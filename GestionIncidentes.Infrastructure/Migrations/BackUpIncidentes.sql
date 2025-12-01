--
-- PostgreSQL database dump
--

-- Dumped from database version 17.4
-- Dumped by pg_dump version 17.4

-- Started on 2025-12-01 10:03:09

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 222 (class 1259 OID 18101)
-- Name: AuditLogs; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."AuditLogs" (
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "Action" text NOT NULL,
    "EntityType" text NOT NULL,
    "EntityId" uuid,
    "Details" text NOT NULL,
    "Timestamp" timestamp with time zone NOT NULL,
    "IpAddress" text NOT NULL
);


ALTER TABLE public."AuditLogs" OWNER TO postgres;

--
-- TOC entry 218 (class 1259 OID 18060)
-- Name: Departments; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Departments" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL
);


ALTER TABLE public."Departments" OWNER TO postgres;

--
-- TOC entry 223 (class 1259 OID 18113)
-- Name: KnowledgeEntries; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."KnowledgeEntries" (
    "Id" uuid NOT NULL,
    "Title" text NOT NULL,
    "Problem" text NOT NULL,
    "Solution" text NOT NULL,
    "Category" text NOT NULL,
    "Tags" text[] NOT NULL,
    "CreatedByUserId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL,
    "UsageCount" integer NOT NULL,
    "IsPublished" boolean NOT NULL,
    "RelatedTicketId" uuid
);


ALTER TABLE public."KnowledgeEntries" OWNER TO postgres;

--
-- TOC entry 224 (class 1259 OID 18125)
-- Name: Notifications; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Notifications" (
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "Title" text NOT NULL,
    "Message" text NOT NULL,
    "Type" text NOT NULL,
    "IsRead" boolean NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "RelatedEntityId" uuid
);


ALTER TABLE public."Notifications" OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 18067)
-- Name: Roles; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Roles" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "Level" integer NOT NULL
);


ALTER TABLE public."Roles" OWNER TO postgres;

--
-- TOC entry 225 (class 1259 OID 18137)
-- Name: TicketReports; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."TicketReports" (
    "Id" uuid NOT NULL,
    "TicketId" uuid NOT NULL,
    "TechnicianId" uuid NOT NULL,
    "ResolutionDetails" text NOT NULL,
    "ProblemDiagnosis" text NOT NULL,
    "ActionsTaken" text NOT NULL,
    "TimeSpent" interval NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "SuggestKnowledgeEntry" boolean NOT NULL
);


ALTER TABLE public."TicketReports" OWNER TO postgres;

--
-- TOC entry 221 (class 1259 OID 18086)
-- Name: Tickets; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Tickets" (
    "Id" uuid NOT NULL,
    "Title" text NOT NULL,
    "Description" text NOT NULL,
    "CreatedByUserId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "Status" text NOT NULL,
    "AffectedParts" text,
    "AffectedType" text DEFAULT ''::text NOT NULL,
    "AssignedToUserId" uuid,
    "Category" text DEFAULT ''::text NOT NULL,
    "EquipmentId" text,
    "ErrorMessage" text,
    "FeedbackComment" text,
    "Location" text DEFAULT ''::text NOT NULL,
    "LocationDetail" text DEFAULT ''::text NOT NULL,
    "Priority" text DEFAULT ''::text NOT NULL,
    "ProblemType" text,
    "ProgramName" text,
    "Rating" integer,
    "ResolvedAt" timestamp with time zone,
    "SlaDeadline" timestamp with time zone,
    "TechnicianNotes" text
);


ALTER TABLE public."Tickets" OWNER TO postgres;

--
-- TOC entry 220 (class 1259 OID 18074)
-- Name: Users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Users" (
    "Id" uuid NOT NULL,
    "Email" text NOT NULL,
    "FullName" text NOT NULL,
    "DepartmentId" uuid NOT NULL,
    "Workload" integer NOT NULL,
    "Role" text NOT NULL,
    "Password" text DEFAULT ''::text NOT NULL
);


ALTER TABLE public."Users" OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 17961)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- TOC entry 4964 (class 0 OID 18101)
-- Dependencies: 222
-- Data for Name: AuditLogs; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."AuditLogs" ("Id", "UserId", "Action", "EntityType", "EntityId", "Details", "Timestamp", "IpAddress") FROM stdin;
366f26e4-357e-46f7-b0bf-f4171fa876b2	00000000-0000-0000-0000-000000000001	AddedKnowledgeEntry	KnowledgeEntry	21b0c8d6-4165-433c-a7a0-69bf2712f7da	Title: Problema con Impresora, Category: Hardware	2025-11-29 02:18:49.271535+00	
1ce5095e-8460-4f0e-b59a-79debe26baed	00000000-0000-0000-0000-000000000001	AddKnowledgeEntryCommand	Command	\N	{"Title":"Problema con Impresora","Problem":"La impresora no imprime","Solution":"Reiniciar el spooler","Category":"Hardware","CreatedByUserId":"00000000-0000-0000-0000-000000000001","RelatedTicketId":null,"Tags":["impresora","hardware"]}	2025-11-29 02:18:49.380151+00	
6d93ec3a-2361-4dd8-be16-70023e1c731f	00000000-0000-0000-0000-000000000001	AddedKnowledgeEntry	KnowledgeEntry	69592fc3-f784-4bd5-91c6-88d167745db8	Title: Error Xampp, Category: Software	2025-11-29 02:18:58.62589+00	
938c0402-e59e-43b3-b13f-293b4656cf72	00000000-0000-0000-0000-000000000001	AddKnowledgeEntryCommand	Command	\N	{"Title":"Error Xampp","Problem":"dwa","Solution":"dwad","Category":"Software","CreatedByUserId":"00000000-0000-0000-0000-000000000001","RelatedTicketId":null,"Tags":[]}	2025-11-29 02:18:58.62901+00	
\.


--
-- TOC entry 4960 (class 0 OID 18060)
-- Dependencies: 218
-- Data for Name: Departments; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Departments" ("Id", "Name") FROM stdin;
11111111-1111-1111-1111-111111111111	Departamento TI
755c0cef-3096-4468-9979-6763977b4fe3	Soporte Técnico
ca2ae91d-f575-4eff-a9b1-7ed9ec6cd1e4	Soporte Técnico
\.


--
-- TOC entry 4965 (class 0 OID 18113)
-- Dependencies: 223
-- Data for Name: KnowledgeEntries; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."KnowledgeEntries" ("Id", "Title", "Problem", "Solution", "Category", "Tags", "CreatedByUserId", "CreatedAt", "UpdatedAt", "UsageCount", "IsPublished", "RelatedTicketId") FROM stdin;
21b0c8d6-4165-433c-a7a0-69bf2712f7da	Problema con Impresora	La impresora no imprime	Reiniciar el spooler	Hardware	{impresora,hardware}	00000000-0000-0000-0000-000000000001	2025-11-29 02:18:48.698314+00	2025-11-29 02:18:48.698315+00	4	t	\N
69592fc3-f784-4bd5-91c6-88d167745db8	Error Xampp	dwa	dwad	Software	{}	00000000-0000-0000-0000-000000000001	2025-11-29 02:18:58.533564+00	2025-11-29 02:18:58.533564+00	3	t	\N
\.


--
-- TOC entry 4966 (class 0 OID 18125)
-- Dependencies: 224
-- Data for Name: Notifications; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Notifications" ("Id", "UserId", "Title", "Message", "Type", "IsRead", "CreatedAt", "RelatedEntityId") FROM stdin;
\.


--
-- TOC entry 4961 (class 0 OID 18067)
-- Dependencies: 219
-- Data for Name: Roles; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Roles" ("Id", "Name", "Level") FROM stdin;
\.


--
-- TOC entry 4967 (class 0 OID 18137)
-- Dependencies: 225
-- Data for Name: TicketReports; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."TicketReports" ("Id", "TicketId", "TechnicianId", "ResolutionDetails", "ProblemDiagnosis", "ActionsTaken", "TimeSpent", "CreatedAt", "SuggestKnowledgeEntry") FROM stdin;
\.


--
-- TOC entry 4963 (class 0 OID 18086)
-- Dependencies: 221
-- Data for Name: Tickets; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Tickets" ("Id", "Title", "Description", "CreatedByUserId", "CreatedAt", "Status", "AffectedParts", "AffectedType", "AssignedToUserId", "Category", "EquipmentId", "ErrorMessage", "FeedbackComment", "Location", "LocationDetail", "Priority", "ProblemType", "ProgramName", "Rating", "ResolvedAt", "SlaDeadline", "TechnicianNotes") FROM stdin;
545b4470-1956-4b74-bda8-2623801a1c76	[SEED] WiFi no disponible	La red WiFi "Universidad_Estudiantes" no aparece en el área de la biblioteca.	09c6afb9-90b7-4e4f-979b-1e40dcd6b513	2025-12-01 00:29:31.433564+00	Pendiente	\N	library	45340476-7486-45a8-a095-b46135605ba4	connectivity	\N	\N	\N	Biblioteca	Segundo piso	critical	\N	\N	\N	\N	2025-12-01 03:29:31.433564+00	\N
58906393-e9c9-424c-bf77-dc52daa919b0	[SEED] Router sin respuesta	El router principal del laboratorio no responde a pings. Los estudiantes no pueden acceder a internet.	e4c9a8de-ccfc-4948-ac7c-bb25b020839b	2025-11-30 23:29:31.432712+00	En proceso	\N	classroom	e1fc4915-214f-4d3f-a346-deb6c894ec76	connectivity	\N	\N	\N	Lab 1		critical	\N	\N	\N	\N	2025-12-01 03:29:31.432813+00	\N
5dbee872-769b-4489-91cb-202cb3a27e41	[SEED] Error en Visual Studio	Visual Studio no compila proyectos y muestra error de MSBuild.	499d3eba-6bc2-43a5-a9d3-7359079531bb	2025-11-30 22:29:31.433437+00	Pendiente	\N	classroom	c53ed966-ea9a-4c9d-a1db-26e92f8746b2	software	\N	MSBuild error: Cannot find SDK	\N	Lab 2		high	software	Visual Studio 2022	\N	\N	2025-12-01 07:29:31.433438+00	\N
651c04ed-602f-430c-bde4-8a65249b35c2	[SEED] Proyector no enciende	El proyector del auditorio no responde al control remoto ni al botón de encendido.	4edc1a6b-216f-4a45-8d0d-552238f0c9f1	2025-11-30 21:29:31.433574+00	En proceso	Proyector	auditorium	45340476-7486-45a8-a095-b46135605ba4	hardware	\N	\N	\N	Auditorio		medium	hardware	\N	\N	\N	2025-12-01 13:29:31.433574+00	Se está verificando la fuente de poder. Posible reemplazo necesario.
9658a684-27c5-4ebc-9bea-8b5eaf74b27b	[SEED] Licencia de AutoCAD vencida	AutoCAD muestra mensaje de licencia vencida en todas las PCs.	e4c9a8de-ccfc-4948-ac7c-bb25b020839b	2025-11-28 01:29:31.433573+00	Resuelto	\N	classroom	e1fc4915-214f-4d3f-a346-deb6c894ec76	software	\N	\N	\N	Lab 3		high	software	AutoCAD 2024	5	2025-11-29 01:29:31.433573+00	2025-11-28 07:29:31.433573+00	Se renovó la licencia educativa con Autodesk.
c4adee49-db08-48f4-b07d-30b221196d1f	[SEED] Monitor parpadeando	El monitor parpadea constantemente.	09c6afb9-90b7-4e4f-979b-1e40dcd6b513	2025-11-29 01:29:31.43357+00	Resuelto	Monitor	classroom	c53ed966-ea9a-4c9d-a1db-26e92f8746b2	hardware	\N	\N	Bien resuelto.	Lab 4		medium	hardware	\N	4	2025-11-30 01:29:31.43357+00	2025-11-29 13:29:31.433571+00	Se ajustó la frecuencia de refresco a 60Hz.
cb5ae16c-afca-4652-967f-f4dc072926eb	[SEED] Monitor sin señal	El monitor de la estación 15 no muestra imagen.	e4c9a8de-ccfc-4948-ac7c-bb25b020839b	2025-11-30 01:29:31.433021+00	Resuelto	Monitor	classroom	45340476-7486-45a8-a095-b46135605ba4	hardware	\N	\N	Excelente servicio, muy rápido.	Lab 3		medium	hardware	\N	5	2025-11-30 13:29:31.433025+00	2025-11-30 13:29:31.433088+00	Se reemplazó el cable HDMI que estaba dañado.
f404293b-68f7-49c9-a8b2-4d12f0cb8642	[SEED] Teclado no responde	El teclado de la PC 08 no funciona correctamente.	499d3eba-6bc2-43a5-a9d3-7359079531bb	2025-11-30 20:29:31.433505+00	En proceso	Teclado	classroom	e1fc4915-214f-4d3f-a346-deb6c894ec76	hardware	PC-08	\N	\N	Lab 1		low	hardware	\N	\N	\N	2025-12-02 01:29:31.433505+00	Se solicitó un teclado nuevo al almacén.
b8e8933d-11f4-4f03-aa72-08ea8e3a33b0	[SEED] Router sin respuesta en Lab 1	El router principal del Laboratorio 1 no responde a pings. Los estudiantes no pueden acceder a internet para realizar sus prácticas de programación.	e4c9a8de-ccfc-4948-ac7c-bb25b020839b	2025-12-01 00:22:47.203977+00	En proceso	\N	classroom	e1fc4915-214f-4d3f-a346-deb6c894ec76	connectivity	\N	\N	\N	Lab 1		critical	\N	\N	\N	\N	2025-12-01 04:22:47.203977+00	\N
c466c230-b4d2-4c86-99cf-c60873536527	[SEED] Monitor sin señal en Lab 3	El monitor de la estación 15 no muestra imagen. La PC enciende correctamente pero no hay señal en pantalla.	e4c9a8de-ccfc-4948-ac7c-bb25b020839b	2025-11-30 02:22:47.203977+00	Resuelto	Monitor	classroom	45340476-7486-45a8-a095-b46135605ba4	hardware	\N	\N	Excelente servicio. El técnico fue muy rápido y profesional.	Lab 3		medium	hardware	\N	5	2025-11-30 14:22:47.203977+00	2025-11-30 14:22:47.203977+00	Se identificó que el cable HDMI estaba dañado. Se reemplazó por uno nuevo y se verificó el funcionamiento correcto del monitor.
ff37b737-b294-44b2-847d-faed9b66e21b	[SEED] Error de compilación en Visual Studio	Visual Studio 2022 no puede compilar proyectos de .NET y muestra error relacionado con MSBuild.	499d3eba-6bc2-43a5-a9d3-7359079531bb	2025-11-30 23:22:47.203977+00	Pendiente	\N	classroom	c53ed966-ea9a-4c9d-a1db-26e92f8746b2	software	\N	MSBuild error: Cannot find SDK Microsoft.NET.Sdk	\N	Lab 2		high	software	Visual Studio 2022	\N	\N	2025-12-01 08:22:47.203977+00	\N
45e0f586-1082-475b-81f3-47718c86df69	[SEED] Teclado no responde en PC-08	El teclado de la PC 08 no funciona correctamente. Algunas teclas no registran entrada.	499d3eba-6bc2-43a5-a9d3-7359079531bb	2025-11-30 21:22:47.203977+00	En proceso	Teclado	classroom	e1fc4915-214f-4d3f-a346-deb6c894ec76	hardware	PC-08	\N	\N	Lab 1		low	hardware	\N	\N	\N	2025-12-02 02:22:47.203977+00	Se solicitó un teclado nuevo al almacén. En espera de llegada del equipo.
f79949bb-71c5-4fe9-a097-f958ce71edc9	[SEED] Red WiFi no disponible en Biblioteca	La red WiFi "Universidad_Estudiantes" no aparece disponible en toda el área de la biblioteca. Los estudiantes no pueden conectarse para estudiar.	09c6afb9-90b7-4e4f-979b-1e40dcd6b513	2025-12-01 01:22:47.203977+00	Pendiente	\N	library	45340476-7486-45a8-a095-b46135605ba4	connectivity	\N	\N	\N	Biblioteca	Segundo piso - Área de lectura	critical	\N	\N	\N	\N	2025-12-01 04:22:47.203977+00	\N
14d7b3ff-bebd-49da-93f5-467655133e42	[SEED] Monitor parpadeando constantemente	El monitor de la estación 22 parpadea constantemente, causando molestias visuales.	09c6afb9-90b7-4e4f-979b-1e40dcd6b513	2025-11-29 02:22:47.203977+00	Resuelto	Monitor	classroom	c53ed966-ea9a-4c9d-a1db-26e92f8746b2	hardware	\N	\N	Bien resuelto, gracias.	Lab 4		medium	hardware	\N	4	2025-11-30 02:22:47.203977+00	2025-11-29 14:22:47.203977+00	Se ajustó la frecuencia de refresco del monitor de 50Hz a 60Hz. Problema solucionado.
7ed28cb9-76a6-4eeb-8790-98ade328ed55	[SEED] Proyector del auditorio no enciende	El proyector del auditorio principal no responde al control remoto ni al botón de encendido físico.	4edc1a6b-216f-4a45-8d0d-552238f0c9f1	2025-11-30 22:22:47.203977+00	En proceso	Proyector	auditorium	45340476-7486-45a8-a095-b46135605ba4	hardware	\N	\N	\N	Auditorio		medium	hardware	\N	\N	\N	2025-12-01 14:22:47.203977+00	Se está verificando la fuente de poder del proyector. Posible reemplazo necesario si la fuente está dañada.
\.


--
-- TOC entry 4962 (class 0 OID 18074)
-- Dependencies: 220
-- Data for Name: Users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Users" ("Id", "Email", "FullName", "DepartmentId", "Workload", "Role", "Password") FROM stdin;
00000000-0000-0000-0000-000000000001	admin@test.com	Administrador Sistema	11111111-1111-1111-1111-111111111111	0	DITIC	password123
00000000-0000-0000-0000-000000000002	tecnico@test.com	Técnico de Soporte	11111111-1111-1111-1111-111111111111	0	Tecnico	password123
00000000-0000-0000-0000-000000000003	estudiante@test.com	Estudiante de Prueba	11111111-1111-1111-1111-111111111111	0	Estudiante	password123
09c6afb9-90b7-4e4f-979b-1e40dcd6b513	pedro.gonzalez@universidad.edu	Pedro González	ca2ae91d-f575-4eff-a9b1-7ed9ec6cd1e4	0	Student	Student123!
45340476-7486-45a8-a095-b46135605ba4	ana.torres@universidad.edu	Ana Torres	ca2ae91d-f575-4eff-a9b1-7ed9ec6cd1e4	0	Technician	Tech123!
499d3eba-6bc2-43a5-a9d3-7359079531bb	maria.lopez@universidad.edu	María López	ca2ae91d-f575-4eff-a9b1-7ed9ec6cd1e4	0	Student	Student123!
4edc1a6b-216f-4a45-8d0d-552238f0c9f1	admin@universidad.edu	Carlos Rodríguez	ca2ae91d-f575-4eff-a9b1-7ed9ec6cd1e4	0	Admin	Admin123!
c53ed966-ea9a-4c9d-a1db-26e92f8746b2	luis.garcia@universidad.edu	Luis García	ca2ae91d-f575-4eff-a9b1-7ed9ec6cd1e4	0	Technician	Tech123!
e1fc4915-214f-4d3f-a346-deb6c894ec76	carlos.mendez@universidad.edu	Carlos Méndez	ca2ae91d-f575-4eff-a9b1-7ed9ec6cd1e4	0	Technician	Tech123!
e4c9a8de-ccfc-4948-ac7c-bb25b020839b	juan.perez@universidad.edu	Juan Pérez	ca2ae91d-f575-4eff-a9b1-7ed9ec6cd1e4	0	Student	Student123!
\.


--
-- TOC entry 4959 (class 0 OID 17961)
-- Dependencies: 217
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20251115230157_InitialCreate	9.0.0
20251115231742_AddUserPassword	9.0.0
20251128192243_AddKnowledgeReportsNotificationsAudit	9.0.0
20251201005409_ExtendTicketEntity	9.0.0
20251201144841_CompleteTicketModelMigration	9.0.0
\.


--
-- TOC entry 4794 (class 2606 OID 18107)
-- Name: AuditLogs PK_AuditLogs; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AuditLogs"
    ADD CONSTRAINT "PK_AuditLogs" PRIMARY KEY ("Id");


--
-- TOC entry 4782 (class 2606 OID 18066)
-- Name: Departments PK_Departments; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Departments"
    ADD CONSTRAINT "PK_Departments" PRIMARY KEY ("Id");


--
-- TOC entry 4798 (class 2606 OID 18119)
-- Name: KnowledgeEntries PK_KnowledgeEntries; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."KnowledgeEntries"
    ADD CONSTRAINT "PK_KnowledgeEntries" PRIMARY KEY ("Id");


--
-- TOC entry 4801 (class 2606 OID 18131)
-- Name: Notifications PK_Notifications; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Notifications"
    ADD CONSTRAINT "PK_Notifications" PRIMARY KEY ("Id");


--
-- TOC entry 4784 (class 2606 OID 18073)
-- Name: Roles PK_Roles; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Roles"
    ADD CONSTRAINT "PK_Roles" PRIMARY KEY ("Id");


--
-- TOC entry 4805 (class 2606 OID 18143)
-- Name: TicketReports PK_TicketReports; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."TicketReports"
    ADD CONSTRAINT "PK_TicketReports" PRIMARY KEY ("Id");


--
-- TOC entry 4791 (class 2606 OID 18092)
-- Name: Tickets PK_Tickets; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Tickets"
    ADD CONSTRAINT "PK_Tickets" PRIMARY KEY ("Id");


--
-- TOC entry 4787 (class 2606 OID 18080)
-- Name: Users PK_Users; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "PK_Users" PRIMARY KEY ("Id");


--
-- TOC entry 4780 (class 2606 OID 17965)
-- Name: __EFMigrationsHistory __EFMigrationsHistory_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "__EFMigrationsHistory_pkey" PRIMARY KEY ("MigrationId");


--
-- TOC entry 4792 (class 1259 OID 18154)
-- Name: IX_AuditLogs_UserId_Timestamp; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_AuditLogs_UserId_Timestamp" ON public."AuditLogs" USING btree ("UserId", "Timestamp");


--
-- TOC entry 4795 (class 1259 OID 18155)
-- Name: IX_KnowledgeEntries_Category; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_KnowledgeEntries_Category" ON public."KnowledgeEntries" USING btree ("Category");


--
-- TOC entry 4796 (class 1259 OID 18156)
-- Name: IX_KnowledgeEntries_CreatedByUserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_KnowledgeEntries_CreatedByUserId" ON public."KnowledgeEntries" USING btree ("CreatedByUserId");


--
-- TOC entry 4799 (class 1259 OID 18157)
-- Name: IX_Notifications_UserId_IsRead; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Notifications_UserId_IsRead" ON public."Notifications" USING btree ("UserId", "IsRead");


--
-- TOC entry 4802 (class 1259 OID 18158)
-- Name: IX_TicketReports_TechnicianId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_TicketReports_TechnicianId" ON public."TicketReports" USING btree ("TechnicianId");


--
-- TOC entry 4803 (class 1259 OID 18159)
-- Name: IX_TicketReports_TicketId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_TicketReports_TicketId" ON public."TicketReports" USING btree ("TicketId");


--
-- TOC entry 4788 (class 1259 OID 18167)
-- Name: IX_Tickets_AssignedToUserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Tickets_AssignedToUserId" ON public."Tickets" USING btree ("AssignedToUserId");


--
-- TOC entry 4789 (class 1259 OID 18168)
-- Name: IX_Tickets_CreatedByUserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Tickets_CreatedByUserId" ON public."Tickets" USING btree ("CreatedByUserId");


--
-- TOC entry 4785 (class 1259 OID 18099)
-- Name: IX_Users_DepartmentId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Users_DepartmentId" ON public."Users" USING btree ("DepartmentId");


--
-- TOC entry 4809 (class 2606 OID 18108)
-- Name: AuditLogs FK_AuditLogs_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AuditLogs"
    ADD CONSTRAINT "FK_AuditLogs_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE RESTRICT;


--
-- TOC entry 4810 (class 2606 OID 18120)
-- Name: KnowledgeEntries FK_KnowledgeEntries_Users_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."KnowledgeEntries"
    ADD CONSTRAINT "FK_KnowledgeEntries_Users_CreatedByUserId" FOREIGN KEY ("CreatedByUserId") REFERENCES public."Users"("Id") ON DELETE RESTRICT;


--
-- TOC entry 4811 (class 2606 OID 18132)
-- Name: Notifications FK_Notifications_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Notifications"
    ADD CONSTRAINT "FK_Notifications_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- TOC entry 4812 (class 2606 OID 18144)
-- Name: TicketReports FK_TicketReports_Tickets_TicketId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."TicketReports"
    ADD CONSTRAINT "FK_TicketReports_Tickets_TicketId" FOREIGN KEY ("TicketId") REFERENCES public."Tickets"("Id") ON DELETE RESTRICT;


--
-- TOC entry 4813 (class 2606 OID 18149)
-- Name: TicketReports FK_TicketReports_Users_TechnicianId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."TicketReports"
    ADD CONSTRAINT "FK_TicketReports_Users_TechnicianId" FOREIGN KEY ("TechnicianId") REFERENCES public."Users"("Id") ON DELETE RESTRICT;


--
-- TOC entry 4807 (class 2606 OID 18169)
-- Name: Tickets FK_Tickets_Users_AssignedToUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Tickets"
    ADD CONSTRAINT "FK_Tickets_Users_AssignedToUserId" FOREIGN KEY ("AssignedToUserId") REFERENCES public."Users"("Id") ON DELETE SET NULL;


--
-- TOC entry 4808 (class 2606 OID 18174)
-- Name: Tickets FK_Tickets_Users_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Tickets"
    ADD CONSTRAINT "FK_Tickets_Users_CreatedByUserId" FOREIGN KEY ("CreatedByUserId") REFERENCES public."Users"("Id") ON DELETE RESTRICT;


--
-- TOC entry 4806 (class 2606 OID 18081)
-- Name: Users FK_Users_Departments_DepartmentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "FK_Users_Departments_DepartmentId" FOREIGN KEY ("DepartmentId") REFERENCES public."Departments"("Id") ON DELETE RESTRICT;


-- Completed on 2025-12-01 10:03:10

--
-- PostgreSQL database dump complete
--


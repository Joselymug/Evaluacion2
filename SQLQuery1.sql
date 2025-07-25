-- Datos de ejemplo
INSERT INTO Users (Username, Email, PasswordHash, FullName) VALUES
('admin', 'admin@test.com', 'hashedpassword123', 'Administrador'),
('dev1', 'dev1@test.com', 'hashedpassword123', 'Desarrollador 1'),
('dev2', 'dev2@test.com', 'hashedpassword123', 'Desarrollador 2');

INSERT INTO Projects (Name, Description, CreatedBy) VALUES
('Sistema Web', 'Desarrollo de sistema web corporativo', 1),
('App Móvil', 'Aplicación móvil para clientes', 1);

INSERT INTO Tasks (Title, Description, Status, Priority, DueDate, ProjectId, AssignedToId, CreatedBy) VALUES
('Diseño de Base de Datos', 'Crear esquema de BD', 'Completada', 'Alta', '2024-01-15', 1, 2, 1),
('API de Autenticación', 'Implementar JWT', 'En Progreso', 'Alta', '2024-01-20', 1, 2, 1),
('Frontend Login', 'Crear pantalla de login', 'Pendiente', 'Media', '2024-01-25', 1, 3, 1);
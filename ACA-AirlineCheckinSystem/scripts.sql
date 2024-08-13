CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL
);

-- Insert 120 names into Users table
INSERT INTO Users (Name)
VALUES 
    ('Name1'), ('Name2'), ('Name3'), ('Name4'), ('Name5'),
    ('Name6'), ('Name7'), ('Name8'), ('Name9'), ('Name10'),
    ('Name11'), ('Name12'), ('Name13'), ('Name14'), ('Name15'),
    ('Name16'), ('Name17'), ('Name18'), ('Name19'), ('Name20'),
    ('Name21'), ('Name22'), ('Name23'), ('Name24'), ('Name25'),
    ('Name26'), ('Name27'), ('Name28'), ('Name29'), ('Name30'),
    ('Name31'), ('Name32'), ('Name33'), ('Name34'), ('Name35'),
    ('Name36'), ('Name37'), ('Name38'), ('Name39'), ('Name40'),
    ('Name41'), ('Name42'), ('Name43'), ('Name44'), ('Name45'),
    ('Name46'), ('Name47'), ('Name48'), ('Name49'), ('Name50'),
    ('Name51'), ('Name52'), ('Name53'), ('Name54'), ('Name55'),
    ('Name56'), ('Name57'), ('Name58'), ('Name59'), ('Name60'),
    ('Name61'), ('Name62'), ('Name63'), ('Name64'), ('Name65'),
    ('Name66'), ('Name67'), ('Name68'), ('Name69'), ('Name70'),
    ('Name71'), ('Name72'), ('Name73'), ('Name74'), ('Name75'),
    ('Name76'), ('Name77'), ('Name78'), ('Name79'), ('Name80'),
    ('Name81'), ('Name82'), ('Name83'), ('Name84'), ('Name85'),
    ('Name86'), ('Name87'), ('Name88'), ('Name89'), ('Name90'),
    ('Name91'), ('Name92'), ('Name93'), ('Name94'), ('Name95'),
    ('Name96'), ('Name97'), ('Name98'), ('Name99'), ('Name100'),
    ('Name101'), ('Name102'), ('Name103'), ('Name104'), ('Name105'),
    ('Name106'), ('Name107'), ('Name108'), ('Name109'), ('Name110'),
    ('Name111'), ('Name112'), ('Name113'), ('Name114'), ('Name115'),
    ('Name116'), ('Name117'), ('Name118'), ('Name119'), ('Name120');

-- Create Flight table
CREATE TABLE Flight (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL
);

-- Insert 1 flight name into Flight table
INSERT INTO Flight (Name)
VALUES ('Flight1');

-- Create FlightUsers table
CREATE TABLE FlightUsers (
    Seat NVARCHAR(10) NOT NULL,
    FlightId INT,
    UserId INT NULL,
    FOREIGN KEY (FlightId) REFERENCES Flight(Id),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- Insert 120 seats into FlightUsers table with null UserId
DECLARE @row INT = 1;
DECLARE @column CHAR(1);

WHILE @row <= 20
BEGIN
    SET @column = 'A';
    WHILE @column <= 'F'
    BEGIN
        INSERT INTO FlightUsers (Seat, FlightId, UserId)
        VALUES (CAST(@row AS NVARCHAR(2)) + @column, 1, NULL);
        SET @column = CHAR(ASCII(@column) + 1);
    END;
    SET @row = @row + 1;
END;
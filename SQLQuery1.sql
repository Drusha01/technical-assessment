IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'assessment_db')
BEGIN
    CREATE DATABASE assessment_db;
END;

use assessment_db;

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'recyclable_items')
BEGIN
	DROP TABLE recyclable_items;
END;
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'recyclables')
BEGIN
	DROP TABLE recyclables;
END;
CREATE TABLE recyclables(
    id INT IDENTITY(1,1) PRIMARY KEY,    
    type_name VARCHAR(100) UNIQUE NOT NULL,       
    rate DECIMAL(18, 2),                  
    min_kg DECIMAL(18, 2),                 
    max_kg DECIMAL(18, 2),                 
	date_created DATETIME DEFAULT CURRENT_TIMESTAMP, 
    date_updated DATETIME DEFAULT CURRENT_TIMESTAMP
);
GO
	CREATE TRIGGER trg_recyclables_update_date_updated
	ON recyclables
	AFTER UPDATE
	AS
	BEGIN
		-- Update the date_updated column to CURRENT_TIMESTAMP
		UPDATE recyclables
		SET date_updated = CURRENT_TIMESTAMP
		FROM recyclables r
		INNER JOIN Inserted i ON r.id = i.id;
	END;
GO

	INSERT INTO recyclables (type_name,rate,min_kg,max_kg)
		VALUES(
		'Type 1',
		5,
		20,500
	),(
	'Type 2',
	10.2,
	50,100
);

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'recyclable_items')
BEGIN
	DROP TABLE recyclable_items;
END;
CREATE TABLE recyclable_items (
    id INT IDENTITY(1,1) PRIMARY KEY,        
    recyclable_type_id INT,                   
    weight DECIMAL(18, 2),                    
    computed_rate DECIMAL(18, 2),            
    item_description VARCHAR(150),          
	date_created DATETIME DEFAULT CURRENT_TIMESTAMP, 
    date_updated DATETIME DEFAULT CURRENT_TIMESTAMP
    FOREIGN KEY (recyclable_type_id) REFERENCES recyclables(id) ON DELETE CASCADE
);

GO
	CREATE TRIGGER trg_recyclable_items_update_date_updated
	ON recyclable_items
	AFTER UPDATE
	AS
	BEGIN
		UPDATE recyclable_items
		SET date_updated = CURRENT_TIMESTAMP
		FROM recyclable_items ri
		INNER JOIN Inserted i ON ri.id = i.id;
	END;
GO



select * from recyclable_items;
select * from recyclables;


CREATE TABLE DB_Scripts (
	id int IDENTITY(1,1) NOT NULL,
	script_id nvarchar(10) NOT NULL,
	success bit NOT NULL,
	output nvarchar(MAX),
	last_execution datetime NOT NULL,
	CONSTRAINT PK_DB_Scripts PRIMARY KEY CLUSTERED 
	(
		id ASC
	)
)

CREATE TABLE Ingredient (
	id int IDENTITY(1,1) NOT NULL,
	name nvarchar(50) NOT NULL,
	unit nvarchar(50) NOT NULL
	CONSTRAINT PK_Ingredient PRIMARY KEY CLUSTERED 
	(
		id ASC
	)
)

CREATE TABLE Recipe (
	id int IDENTITY(1,1) NOT NULL,
	name nvarchar(50) NOT NULL,
	instructions nvarchar(1500) NOT NULL
	CONSTRAINT PK_Recipe PRIMARY KEY CLUSTERED 
	(
		id ASC
	)
)

CREATE TABLE Recipe_Ingredient (
	id int IDENTITY(1,1) NOT NULL,
	recipe_id int NOT NULL,
	ingredient_id int NOT NULL,
	amount int NOT NULL,
	CONSTRAINT PK_Recipe_Ingredient PRIMARY KEY CLUSTERED 
	(
		id ASC
	),
	FOREIGN KEY (recipe_id) REFERENCES Recipe(id) ON DELETE CASCADE,
	FOREIGN KEY (ingredient_id) REFERENCES Ingredient(id) ON DELETE CASCADE
)
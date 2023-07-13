CREATE TABLE DB_Scripts (
	script_id char(10) NOT NULL,
	success bit NOT NULL,
	output nvarchar(MAX),
	last_execution datetime NOT NULL
)

CREATE TABLE Ingredient (
	id int NOT NULL UNIQUE,
	name nvarchar(50) NOT NULL,
	unit nvarchar(50) NOT NULL
	PRIMARY KEY (id)
)

CREATE TABLE Recipe (
	id int NOT NULL UNIQUE,
	name nvarchar(50) NOT NULL,
	instructions nvarchar(1500) NOT NULL
	PRIMARY KEY (id)
)

CREATE TABLE Recipe_Ingredient (
	id int NOT NULL UNIQUE,
	recipe_id int NOT NULL,
	ingredient_id int NOT NULL,
	amount int NOT NULL,
	PRIMARY KEY (id),
	FOREIGN KEY (recipe_id) REFERENCES Recipe(id),
	FOREIGN KEY (ingredient_id) REFERENCES Ingredient(id)
)
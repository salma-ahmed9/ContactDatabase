CREATE MIGRATION m1cvlrnheyrz62ivo5jgoifsfr4w64iztlluvgfiqubtcezfwqhxca
    ONTO initial
{
  CREATE TYPE default::Contact {
      CREATE REQUIRED PROPERTY birthdate: std::str;
      CREATE REQUIRED PROPERTY description: std::str;
      CREATE REQUIRED PROPERTY email: std::str;
      CREATE REQUIRED PROPERTY first_name: std::str;
      CREATE REQUIRED PROPERTY last_name: std::str;
      CREATE REQUIRED PROPERTY marriage_status: std::bool;
      CREATE REQUIRED PROPERTY title: std::str;
  };
};

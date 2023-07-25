CREATE MIGRATION m1y5dk2w7xvyw5pnrqxztgtvbwo6gioeuvo5vbx6epzph4ecnrmuxq
    ONTO m1cvlrnheyrz62ivo5jgoifsfr4w64iztlluvgfiqubtcezfwqhxca
{
  ALTER TYPE default::Contact {
      ALTER PROPERTY birthdate {
          RENAME TO birth_date;
      };
  };
};

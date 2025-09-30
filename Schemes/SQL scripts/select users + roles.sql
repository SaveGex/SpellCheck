USE [spell_test_db]

SELECT TOP (1000) u.Id
      ,u.Username
      ,u.PasswordHash
      ,u.Number
      ,u.Email
      ,u.Created_At
      ,u.Deleted_At
	  ,r.Name	AS [Role Name]
  FROM Users u LEFT JOIN RoleUser ru ON u.Id = ru.UsersId
	LEFT JOIN Roles r ON ru.RolesId = r.Id;
	
  

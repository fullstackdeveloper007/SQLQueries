# Brush up your knowledge on SQL 
 
<b>1. What is the command used to fetch first 5 characters of the string?</b><br/>
There are many ways to fetch first 5 characters of the string - <br/>
a) Select SUBSTRING(StudentName,1,5) as studentname from student <br/>
b) Select RIGHT(Studentname,5) as studentname from student
<br/>
<br/> <b>2. How to create a Foreign key </b>


a). CREATE TABLE Orders (<br/>
     OrderID int NOT NULL PRIMARY KEY,<br/>
    OrderNumber int NOT NULL,<br/>
    PersonID int FOREIGN KEY REFERENCES Persons(PersonID) <br/>
);<br/>

b). ALTER TABLE Orders
ADD FOREIGN KEY (PersonID) REFERENCES Persons(PersonID);

c) ALTER TABLE Orders
ADD CONSTRAINT FK_PersonOrder
FOREIGN KEY (PersonID) REFERENCES Persons(PersonID);
			
<br/>

<b>3. Handle Exception in SQL</b><br/>
<p>BEGIN TRY <br/>
DECLARE @Mynum INT <br/>
---- Divide by zero Error<br/>
  SET @Mynum = 10/0<br/>
  PRINT 'This will not execute'<br/>
END TRY<br/>
BEGIN CATCH<br/>
SELECT @@Error, @@ROWCOUNT<br/>
SELECT ERROR_NUMBER() AS ErrorNumber, ERROR_SEVERITY() AS ErrorSeverity, ERROR_STATE() AS ErrorState, ERROR_PROCEDURE() AS ErrorProcedure, ERROR_LINE() AS ErrorLine, ERROR_MESSAGE() AS ErrorMessage;<br/>
END CATCH;<br/>
GO <p/><br/>


<b>4.  RaiseError()</b><br/>
The RAISERROR statement allows you to generate your own error messages and return these messages back to the application using the same format as a system error or warning message generated by SQL Server Database Engine.<br/><b>Syntax : </b>RAISERROR ( { message_id | message_text | @local_variable }  
    { ,severity ,state }  
    [ ,argument [ ,...n ] ] )  
    [ WITH option [ ,...n ] ];

	 <br/>

<b>5. SQL Transactions</b> <br/>
<pre class="notranslate">
	<code> 
BEGIN TRANSACTION T1	
	INSERT INTO Customer VALUES (10, 'Code_10', 'Ramesh')
	INSERT INTO Customer VALUES (11, 'Code_11', 'Suresh') 
BEGIN TRANSACTION T2
	INSERT INTO Customer VALUES (12, 'Code_12', 'Priyanka')
  	INSERT INTO Customer VALUES (13, 'Code_13', 'Preety') 
  	PRINT @@TRANCOUNT --** Here TRANCOUNT value 2**
COMMIT TRANSACTION T2 -- This does not physically commit
  	PRINT @@TRANCOUNT -- **Here TRANCOUNT value 1**
COMMIT TRANSACTION T1 -- This does a physically commit
  	PRINT @@TRANCOUNT --** Here TRANCOUNT value 0		**
</code></pre>
6.  

<pre class="notranslate">
	
</pre>

 


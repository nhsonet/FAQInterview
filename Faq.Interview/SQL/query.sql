
SELECT
	ROW_NUMBER() OVER(PARTITION BY Rank ORDER BY Salary DESC, Rank, Id) AS 'Serial No', datatab.*
FROM
(
	SELECT
		e1.Id, e1.Rank, e1.Designation, e1.Salary,
		CASE
			WHEN ((MAX(e2.Salary) - e1.Salary) > 0) THEN CAST((MAX(e2.Salary) - e1.Salary) AS NVARCHAR(MAX))
			ELSE 'Max Salary'
		END AS 'Salary Difference'
	FROM Employee e1
	INNER JOIN
	(
		SELECT Rank, Max(Salary) Salary
		FROM Employee
		GROUP BY Rank, Designation
		--ORDER BY Rank OFFSET 0 ROWS
	) e2 ON e1.Rank = e2.Rank
	WHERE
		e1.Rank < 3
	GROUP BY
		e1.Id, e1.Rank, e1.Designation, e1.Salary
	--ORDER BY e1.Salary DESC OFFSET 0 ROWS
) datatab

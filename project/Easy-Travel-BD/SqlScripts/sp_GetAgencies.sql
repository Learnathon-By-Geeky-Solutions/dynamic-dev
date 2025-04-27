IF OBJECT_ID('sp_GetAgencies', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetAgencies;
GO

CREATE PROCEDURE sp_GetAgencies
    @SearchTerm NVARCHAR(100),
    @SortColumn NVARCHAR(50),
    @SortOrder NVARCHAR(4),
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
    SET NOCOUNT ON;

    WITH FilteredAgencies AS
    (
        SELECT *,
               ROW_NUMBER() OVER (
                   ORDER BY 
                       CASE 
                           WHEN @SortColumn = 'Name' AND @SortOrder = 'ASC' THEN Name 
                       END ASC,
                       CASE 
                           WHEN @SortColumn = 'Name' AND @SortOrder = 'DESC' THEN Name 
                       END DESC,
					   CASE 
                           WHEN @SortColumn = 'Address' AND @SortOrder = 'ASC' THEN Name 
                       END ASC,
                       CASE 
                           WHEN @SortColumn = 'Address' AND @SortOrder = 'DESC' THEN Name 
                       END DESC,
                       CASE 
                           WHEN @SortColumn = 'AddDate' AND @SortOrder = 'ASC' THEN AddDate 
                       END ASC,
                       CASE 
                           WHEN @SortColumn = 'AddDate' AND @SortOrder = 'DESC' THEN AddDate 
                       END DESC,
					    CASE 
                           WHEN @SortColumn = 'AddDate' AND @SortOrder = 'ASC' THEN AddDate 
                       END ASC,
                       CASE 
                           WHEN @SortColumn = 'AddDate' AND @SortOrder = 'DESC' THEN AddDate 
                       END DESC
               ) AS RowNum
        FROM Agencies
        WHERE
            (@SearchTerm IS NULL OR
             Name LIKE '%' + @SearchTerm + '%' OR
             Address LIKE '%' + @SearchTerm + '%' OR
             ContactNumber LIKE '%' + @SearchTerm + '%' OR
             Website LIKE '%' + @SearchTerm + '%' OR
             LicenseNumber LIKE '%' + @SearchTerm + '%')
    )
    SELECT *
    FROM FilteredAgencies
    WHERE RowNum BETWEEN ((@PageNumber - 1) * @PageSize + 1) AND (@PageNumber * @PageSize)
    ORDER BY RowNum;
END
GO

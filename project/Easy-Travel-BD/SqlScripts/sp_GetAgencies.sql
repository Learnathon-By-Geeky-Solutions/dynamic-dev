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

    DECLARE @SQL NVARCHAR(MAX);

    SET @SQL = '
    WITH FilteredAgencies AS
    (
        SELECT *,
               ROW_NUMBER() OVER (ORDER BY ' + QUOTENAME(@SortColumn) + ' ' + @SortOrder + ') AS RowNum
        FROM Agencies
        WHERE
            (@SearchTerm IS NULL OR
             Name LIKE ''%'' + @SearchTerm + ''%'' OR
             Address LIKE ''%'' + @SearchTerm + ''%'' OR
             ContactNumber LIKE ''%'' + @SearchTerm + ''%'' OR
             Website LIKE ''%'' + @SearchTerm + ''%'' OR
             LicenseNumber LIKE ''%'' + @SearchTerm + ''%'')
    )
    SELECT *
    FROM FilteredAgencies
    WHERE RowNum BETWEEN ((' + CAST(@PageNumber AS NVARCHAR) + ' - 1) * ' + CAST(@PageSize AS NVARCHAR) + ' + 1) AND (' + CAST(@PageNumber AS NVARCHAR) + ' * ' + CAST(@PageSize AS NVARCHAR) + ')
    ORDER BY RowNum;';

    EXEC sp_executesql @SQL, N'@SearchTerm NVARCHAR(100)', @SearchTerm;
END

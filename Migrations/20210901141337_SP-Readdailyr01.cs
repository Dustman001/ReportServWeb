using Microsoft.EntityFrameworkCore.Migrations;

namespace ReportServiceWeb02.Migrations
{
    public partial class SPReaddailyr01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Areports",
                type: "nvarchar(50)",
                nullable: true);

			var sp = @"CREATE PROCEDURE [dbo].[readdailyr](@usedate datetime, @type int)
				AS
				BEGIN

				CREATE TABLE #DRep(
					[Time]		[datetime]		NULL,
					[Message]	[varchar](500)	NOT NULL,
					[Station]	[varchar](50)	NOT NULL,
					[Point]		[varchar](50)	NOT NULL,
					[Operator]	[varchar](50)	NOT NULL,
					[CTIME]		[datetime]		NULL,
					[STATE]	[varchar](50)	NOT NULL
				)

				CREATE TABLE #DINC(
					[Time]		[datetime]		NULL,
					[Message]	[varchar](500)	NOT NULL,
					[Station]	[varchar](50)	NOT NULL,
					[Point]		[varchar](50)	NOT NULL,
					[Operator]	[varchar](50)	NOT NULL,
					[Estado]	[varchar](50)	NOT NULL,
					[CTime]		[datetime]		NULL,
					[Seconds]	[int]			NULL,
					[STATE]	[varchar](50)	NOT NULL
				)
					SET NOCOUNT ON;

				set @usedate = dateadd(minute, 1, @usedate);

				insert into #DRep
				SELECT [TimeStamp],[Message], isnull([Station],''), isnull([Point],''), [Operator], [CapturTime], [STATE]
				FROM [areports] where ([CapturTime] between dateadd(minute, -1441, @usedate) and dateadd(second, -61, @usedate) 
				or [TimeStamp] between @usedate and dateadd(second, -61, @usedate))
				order by [CapturTime], [TimeStamp] asc

				set @usedate = DATEADD(MI,-1,@usedate);

				delete from #DRep where Time < DATEADD(DAY, -1, @usedate)
				delete from #DRep where Time > @usedate
				delete from #DRep where CTIME > @usedate

				insert into #DINC
				select Distinct Time, Message, Station, Point, Operator, '', CTIME, DATEDIFF(Second, Time, CTIME), [STATE] 
				from #DRep where Point = '52' and ([STATE] = 0 or [STATE] = 1) order by CTIME, Time asc

				update #DINC set Estado = 'Abierto' where Message like '%Abierto' or Message like '%Estado = Abierto%';
				update #DINC set Estado = 'Cerrado' where Message like '%Cerrado' or Message like '%Estado = Cerrado%';

				update #DINC set Time = dateadd(second, 11716, Time) where Station = 'TIRE701' and Seconds > 11700
				update #DINC set Time = dateadd(second, 12307, Time) where Station = 'NJUD701' and Seconds > 12200
				update #DINC set Seconds = DATEDIFF(S, Time, CTime)

				delete from #DINC where [Message] like '%Estado = Cerrado (Cerrado%' or [Message] like '%Estado = Abierto (Abierto%';

				IF @type = 0
				BEGIN
				SELECT DISTINCT [Time], [Station], [Estado], [CTIME] FROM #DINC WHERE (([Estado] like 'Abierto') or ([Estado] like 'Cerrado')) ORDER BY [Station] ASC, [CTIME], [Time] ASC;
				END
				ELSE IF @type = 1
				BEGIN
				SELECT DISTINCT [Time], [Message], [Station], [Operator], [CTIME] FROM #DRep ORDER BY [CTIME], [Time] ASC;
				END
				ELSE IF @type = 2
				BEGIN
				SELECT DISTINCT CONCAT([Time],'\t',[Station],'\t',[Estado],'\t',[CTime],'\n') as 'Data', [Station], [CTIME], [Time] 
				FROM #DINC WHERE ([Estado] like 'Abierto' or [Estado] like 'Cerrado') ORDER BY [Station] ASC, [CTIME], [Time] ASC
				END
				ELSE IF @type = 3
				BEGIN
				SELECT DISTINCT CONCAT([Time],'\t',[Message],'\t',[Station],'\t',[Operator],'\t',[CTime],'\n') as 'Data', 
				[CTIME], [Time] FROM #DRep ORDER BY [CTIME], [Time] ASC
				END
				ELSE IF @type = 4
				BEGIN

				declare @INC_SCADA table(
					[Id] [int] IDENTITY(1,1) NOT NULL,
					[Time] [datetime2](3) NULL,
					[Station] [varchar](50) NULL,
					[Estado] [varchar](50) NULL,
					[CTime] [datetime2](3) NULL,
					[sequence] [int] NULL
				)

				Insert into @INC_SCADA
				SELECT DISTINCT [Time], [Station], [Estado], [CTIME], RANK() OVER(
				Partition by Station
				Order by [CTIME] ASC, [Time] ASC
				) FROM #DINC WHERE (([Estado] like 'Abierto') or ([Estado] like 'Cerrado')) ORDER BY [Station] ASC, [CTIME], [Time] ASC;

				declare fcursor cursor for
				select distinct Station from @INC_SCADA

				declare @smt varchar(50), @est varchar(50), @seq int, @date datetime2

				open fcursor

				fetch next from fcursor into
				@smt

				while @@FETCH_STATUS = 0
				BEGIN

					select top 1 @est = Estado, @seq = sequence, @date = Time from @INC_SCADA
					where station = @smt order by Time asc

					IF @est = 'Cerrado'
					BEGIN
					   insert into @INC_SCADA
					   select Convert(varchar, @date, 111), @smt, 'Abierto', Convert(varchar, @date, 111), @seq-1
					END

					select top 1 @est = Estado, @seq = sequence, @date = Time from @INC_SCADA
					where station = @smt order by Time DESC

					IF @est = 'Abierto'
					BEGIN
					   insert into @INC_SCADA
					   select dateadd(DD, 1, Convert(varchar, @date, 111)), @smt, 'Cerrado', dateadd(DD, 1, Convert(varchar, @date, 111)), @seq+1
					END

					fetch next from fcursor into
					@smt

				END
				close fcursor
				deallocate fcursor
				select is1.Station, is1.sequence, is1.Time, is2.Time from @INC_SCADA as is1
				left join @INC_SCADA as is2 on is2.Station = is1.station and is2.sequence = is1.sequence+1 and is2.Estado = 'Cerrado'
				where is1.Estado = 'Abierto'
				order by is1.Station, is1.sequence
				END

				DROP table #DINC
				drop table #DRep

				END";

			migrationBuilder.Sql(sp);
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Areports");

			migrationBuilder.Sql("DROP PROCEDURE [dbo].[readdailyr];");
		}
    }
}

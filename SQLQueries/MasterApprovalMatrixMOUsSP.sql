CREATE procedure SP_GetAllApprovalMatrixMOUDto @pageNumber int, @pageSize int
as
begin
declare @totalRecords int
	select 
		@totalRecords = count(MasterApprovalMatrixMOUId) 
	from 
		MasterApprovalMatrixMOUs
 
	select 
		mmc.MasterApprovalMatrixMOUId,
		d.DepartmentName,
		me1.EmployeeName as ApproverName1,
		me2.EmployeeName as ApproverName2,
		me3.EmployeeName as ApproverName3,
		@totalRecords as TotalRecords
	from 
		MasterApprovalMatrixMOUs as mmc 
		inner join 
		Departments as d on mmc.DepartmentId = d.DepartmentId
		inner join
		MasterEmployees as me1 on mmc.ApproverId1 = me1.EmployeeCode
		inner join 
		MasterEmployees as me2 on mmc.ApproverId2 = me2.EmployeeCode
		inner join 
		MasterEmployees as me3 on mmc.ApproverId3 = me3.EmployeeCode
	order by mmc.MasterApprovalMatrixMOUId
	offset (@pageNumber-1)*@pageSize rows
	fetch next @pageSize rows only
end

-- Get By Id
CREATE procedure SP_GetApprovalMatrixMOUById @id int
as
begin
	declare @count int
	select 
		@count = count(MasterApprovalMatrixMOUId)
	from
		MasterApprovalMatrixMOUs
	where 
		MasterApprovalMatrixMOUId = @id
 
	if @count = 1
	begin
		select 
			mmc.MasterApprovalMatrixMOUId,
			mmc.DepartmentId,
			d.DepartmentName,
			me1.EmployeeName as ApproverName1,
			me1.EmployeeCode as ApproverId1,
			me2.EmployeeName as ApproverName2,
			me2.EmployeeCode as ApproverId2,
			me3.EmployeeName as ApproverName3,
			me3.EmployeeCode as ApproverId3,
			mmc.NumberOfDays
		from 
			MasterApprovalMatrixMOUs as mmc 
			inner join 
			Departments as d on mmc.DepartmentId = d.DepartmentId
			inner join
			MasterEmployees as me1 on mmc.ApproverId1 = me1.EmployeeCode
			inner join 
			MasterEmployees as me2 on mmc.ApproverId2 = me2.EmployeeCode
			inner join 
			MasterEmployees as me3 on mmc.ApproverId3 = me3.EmployeeCode
		where
			MasterApprovalMatrixMOUId = @id
	end
	else
	begin
		declare @message varchar(50)
		set @message = 'Approval MOU with value ID ' + cast(@id as varchar(20)) + ' not found'
		raiserror(@message,16,1);
	end
end

--Update
CREATE procedure SP_UpdateApprovalMatrixMOU @id int, @approverId1 varchar(20), @approverId2 varchar(20), @approverId3 varchar(20), @numberOfDays int
as
begin
	declare @count int
	select 
		@count = count(MasterApprovalMatrixMOUId)
	from
		MasterApprovalMatrixMOUs
	where 
		MasterApprovalMatrixMOUId = @id
 
	if @count = 1
	begin
		update 
			MasterApprovalMatrixMOUs
		set 
			ApproverId1 = @approverId1,
			ApproverId2 = @approverId2,
			ApproverId3 = @approverId3,
			NumberOfDays = @numberOfDays
		where
			MasterApprovalMatrixMOUId = @id
	end
	else
	begin
		declare @message varchar(50)
		set @message = 'Approval MOU with value ID ' + cast(@id as varchar(20)) + ' not found'
		raiserror(@message,16,1);
	end
end
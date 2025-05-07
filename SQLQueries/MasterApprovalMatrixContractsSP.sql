CREATE procedure SP_GetAllApprovalMatrixContract @pageNumber int, @pageSize int
as
begin
declare @totalRecords int
	select 
		@totalRecords = count(MasterApprovalMatrixContractId) 
	from 
		MasterApprovalMatrixContracts
 
	select 
		mmc.MasterApprovalMatrixContractId,
		d.DepartmentName,
		me1.EmployeeName as ApproverName1,
		me2.EmployeeName as ApproverName2,
		me3.EmployeeName as ApproverName3,
		@totalRecords as TotalRecords
	from 
		MasterApprovalMatrixContracts as mmc 
		inner join 
		Departments as d on mmc.DepartmentId = d.DepartmentId
		inner join
		MasterEmployees as me1 on mmc.ApproverId1 = me1.EmployeeCode
		inner join 
		MasterEmployees as me2 on mmc.ApproverId2 = me2.EmployeeCode
		inner join 
		MasterEmployees as me3 on mmc.ApproverId3 = me3.EmployeeCode
	order by mmc.MasterApprovalMatrixContractId
	offset (@pageNumber-1)*@pageSize rows
	fetch next @pageSize rows only
end

-- Get By ID
CREATE procedure SP_GetApprovalMatrixContractById @id int
as
begin
	declare @count int
	select 
		@count = count(MasterApprovalMatrixContractId)
	from
		MasterApprovalMatrixContracts
	where 
		MasterApprovalMatrixContractId = @id
 
	if @count = 1
	begin
		select 
			mmc.MasterApprovalMatrixContractId,
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
			MasterApprovalMatrixContracts as mmc 
			inner join 
			Departments as d on mmc.DepartmentId = d.DepartmentId
			inner join
			MasterEmployees as me1 on mmc.ApproverId1 = me1.EmployeeCode
			inner join 
			MasterEmployees as me2 on mmc.ApproverId2 = me2.EmployeeCode
			inner join 
			MasterEmployees as me3 on mmc.ApproverId3 = me3.EmployeeCode
		where
			MasterApprovalMatrixContractId = @id
	end
	else
	begin
		declare @message varchar(50)
		set @message = 'Approval contract with value ID ' + cast(@id as varchar(20)) + ' not found'
		raiserror(@message,16,1);
	end
end

-- Update
CREATE procedure SP_UpdateApprovalMatrixContract @id int, @ApproverId1 varchar(20), @ApproverId2 varchar(20), @ApproverId3 varchar(20), @NumberOfDays int
as
begin
	declare @count int
	select 
		@count = count(MasterApprovalMatrixContractId)
	from
		MasterApprovalMatrixContracts
	where 
		MasterApprovalMatrixContractId = @id
 
	if @count = 1
	begin
		update 
			MasterApprovalMatrixContracts
		set 
			ApproverId1 = @ApproverId1,
			ApproverId2 = @ApproverId2,
			ApproverId3 = @ApproverId3,
			NumberOfDays = @NumberOfDays
		where
			MasterApprovalMatrixContractId = @id
	end
	else
	begin
		declare @message varchar(50)
		set @message = 'Approval contract with value ID ' + cast(@id as varchar(20)) + ' not found'
		raiserror(@message,16,1);
	end
end
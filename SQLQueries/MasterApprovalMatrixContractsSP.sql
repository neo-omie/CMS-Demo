USE CMS_Trailblazers
-- Get All Approval Matrix Contract
CREATE OR ALTER PROCEDURE SP_GetAllApprovalMatrixContract @pageNumber int, @pageSize int
AS
BEGIN
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

-- Get Approval Matrix Contract By ID
CREATE OR ALTER PROCEDURE SP_GetApprovalMatrixContractById @id int
AS
BEGIN
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

-- Update Approval Matrix Contract
CREATE OR ALTER PROCEDURE SP_UpdateApprovalMatrixContract 
@id int, 
@ApproverId1 varchar(20), 
@ApproverId2 varchar(20), 
@ApproverId3 varchar(20), 
@NumberOfDays int, 
@UpdatedBy nvarchar(20),
@ForTable int,
@Status int
AS
BEGIN
	declare @count int
	declare @currentApproverId1 nvarchar(50)
	declare @currentApproverId2 nvarchar(50)
	declare @currentApproverId3 nvarchar(50)
	declare @currentNumberOfDays int
	declare @description nvarchar(200)
	declare @isChanged int
	set @isChanged = 0
	set @description = @UpdatedBy + ' updated'
	select 
		@count = count(MasterApprovalMatrixContractId)
	from
		MasterApprovalMatrixContracts
	where 
		MasterApprovalMatrixContractId = @id
 
	if @count = 1
	begin
		select
			@currentApproverId1 = ApproverId1,
			@currentApproverId2 = ApproverId2,
			@currentApproverId3 = ApproverId3,
			@currentNumberOfDays = NumberOfDays
		from
			MasterApprovalMatrixContracts
		where 
			MasterApprovalMatrixContractId = @id

		if @currentApproverId1 <> @ApproverId1
		begin 
			set @description = @description + ' approver1 from ' + @currentApproverId1 + ' to ' + @ApproverId1
			set @isChanged = 1
		end
		if @currentApproverId2 <> @ApproverId2
		begin 
			set @description = @description + ' approver2 from ' + @currentApproverId2 + ' to ' + @ApproverId2
			set @isChanged = 1
		end
		if @currentApproverId3 <> @ApproverId3
		begin 
			set @description = @description + ' approver3 from ' + @currentApproverId3 + ' to ' + @ApproverId3
			set @isChanged = 1
		end
		if @currentNumberOfDays <> @NumberOfDays
		begin 
			set @description = @description + ' number of days from ' + @currentNumberOfDays + ' to ' + @NumberOfDays
			set @isChanged = 1
		end
		begin try
			begin transaction
				if @isChanged = 1
				begin
					update 
						MasterApprovalMatrixContracts
					set 
						ApproverId1 = @ApproverId1,
						ApproverId2 = @ApproverId2,
						ApproverId3 = @ApproverId3,
						NumberOfDays = @NumberOfDays,
						UpdatedBy = @UpdatedBy,
						UpdateOn = getdate()
					where
						MasterApprovalMatrixContractId = @id;

					insert into AuditTrails 
						(TableId,ForTable,ActionDescription,LogTime,LoggedBy,Status)
					values 
						(@id, @ForTable, @description,getdate(),@UpdatedBy,@Status);

				end
			commit transaction
		end try
		begin catch
			rollback transaction
		end catch
	end
	else
	begin
		declare @message varchar(50)
		set @message = 'Approval contract with value ID ' + cast(@id as varchar(20)) + ' not found'
		raiserror(@message,16,1);
	end
end
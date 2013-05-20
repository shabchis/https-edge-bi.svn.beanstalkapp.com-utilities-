DECLARE	@return_value int,
		@returnMsg nvarchar(4000)

EXEC	@return_value = [dbo].[CLR_Alerts_ConversionAnalysis_Adgroup]
		@AccountID = 7,
		@Period = 30,
		@ToDay = N'2013-01-12 00:00:00',
		@ChannelID = NULL,
		@CPR_threshold = 2,
		@CPA_threshold = 1.5,
		@excludeIds = N'-1',
		@cubeName = N'BOEasyForex2',
		@acq1FieldName = N'Regs',
		@acq2FieldName = N'Actives',
		@cpaFieldName = N'Cost/Active',
		@cprFieldName = N'Cost/Reg',
		@returnMsg = @returnMsg OUTPUT,
		@extraFields = NULL

SELECT	@returnMsg as N'@returnMsg'

SELECT	'Return Value' = @return_value

GO

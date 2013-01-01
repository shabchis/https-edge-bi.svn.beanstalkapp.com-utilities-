DECLARE	@return_value int,
		@returnMsg nvarchar(4000)

EXEC	@return_value = [dbo].[CLR_Alerts_ConversionAnalysis]
		@AccountID = 7,
		@Period = 30,
		@ToDay = N'2012-06-02 00:00:00',
		@ChannelID = NULL,
		@threshold = 2,
		@excludeIds = '4004457,4002683,4002762,4004836',
		@cubeName = N'BOEasyForex2',
		@acqFieldName = N'Actives',
		@cpaFieldName = N'Cost/Active',
		@returnMsg = @returnMsg OUTPUT

SELECT	@returnMsg as N'@returnMsg'

SELECT	'Return Value' = @return_value


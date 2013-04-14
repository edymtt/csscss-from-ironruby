require 'csscss'

class Helper
	def run file
	  redundancies = Csscss::RedundancyAnalyzer.new(file).redundancies(
          minimum:            1
        )

          report = Csscss::Reporter.new(redundancies).report(verbose:@verbose, color:@color)
	end
end
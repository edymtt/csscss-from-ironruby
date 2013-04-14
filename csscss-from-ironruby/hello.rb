require "stringio"

puts 'Hello from file'

# method_missing_demo.rb
class MethodMissingDemo
  def print_all(args)
    args.map {|arg| puts arg}
  end

  def method_missing(name, *args)
    name.to_s.gsub(/([[:lower:]\d])([[:upper:]])/,'\1 \2')
  end
end
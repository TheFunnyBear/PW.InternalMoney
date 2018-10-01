Feature: RegisterUser
	In this test we check that new user created 

@accountFunctionalTest
Scenario: Create new user
	Given Navigate to main page
	And Navigate to register form
	And Entered User first name
	And Entered User last name
	And Entered User email
	And Entered Password
	And Entered Confirm password
	When Press Register
	Then User account created

@accountFunctionalTest
Scenario: Login as new user
	Given Navigate to main page
	And Navigate to login form
	And Entered User email
	And Entered Password
	When Press Login
	Then User Logged in


@accountFunctionalTest
Scenario: Create some users
	Given Navigate to main page
	When Create some users
	Then Users created


@accountFunctionalTest
Scenario: Transfer some money
	Given Navigate to main page
	And Login User 2
	And Remember Balance
	And Logout User 2
	And Login User 1
	And Navigate to create
	And Transfer to User 2 money 100.01
	And Logout User1
	When Login User2
	Then Check Balance added 100.01

@accountFunctionalTest
Scenario: You can not transfer money to yourself
	Given Navigate to main page
	And Login User 1
	And Navigate to create
	When Transfer to User 1 money 33.35
	Then Can not transfer money to yourself

@accountFunctionalTest
Scenario: Try transfer money without money
	Given Navigate to main page
	And Login User 3
	And Navigate to create
	And Transfer to User 4 money 500
	When Navigate to create
	Then No money

@accountFunctionalTest
Scenario: Try transfer money and check balance
	Given Navigate to main page
	And Login User 4
	And Remember Balance
	And Navigate to create
	When Transfer to User 3 money 500
	Then Check Balance less 500
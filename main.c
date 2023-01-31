#include <stdio.h>
#include <stdlib.h>
#include <time.h>

int main()
{
    //Create a random number to guess, and a variable for input
    time_t t;
    srand((unsigned)time(&t));
    //int randomNumber = rand()%21;
    int randomNumber = 7;
    int input;

    //Tells the player the conditions of the game
    printf("This is a guessing game. \n");
    printf("I have chosen a number between 0 and 20 which you must guess.\n\n");

    for(int i = 5; i > 0; i--)
    {
        //Prompts the player for a guess and tells how many tries remain
        printf("You have %d tries left.\n", i);
        printf("Enter your guess: ");
        scanf("%d", &input);

        //Prints a win message and exits the program if the guess is correct
        if(input == randomNumber)
        {
            printf("\nYou win!!!");
            return 0;
        }
        //Prompts the player to guess lower
        else if(input > randomNumber)
        {
            printf("Sorry my number is less than that.");
        }
        //Prompts the player to guess higher
        else if(input < randomNumber)
        {
            printf("Sorry my number is greater than that.");
        }
    }
    //Prints a lose message if the player fails to guess in five tries
    printf("\nSorry, better luck next time...\nGAME OVER");
    return 0;
}



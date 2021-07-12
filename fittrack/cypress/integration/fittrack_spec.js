
function goToSetsPage() {
    cy.visit('http://localhost:4200/workout')
    cy.get('.start-workout > a').click()
}

function verifyContentsOfSet(setContents) {
    cy.get('input[formcontrolname="exerciseType"').should('have.value', setContents.exerciseType)
    cy.get('input[formcontrolname="plannedReps"').should('have.value', setContents.plannedReps)
    cy.get('input[formcontrolname="plannedWeight"').should('have.value', setContents.plannedWeight)
    cy.get('input[formcontrolname="actualReps"').should('have.value', setContents.actualReps)
    cy.get('input[formcontrolname="actualWeight"').should('have.value', setContents.actualWeight)
}

function clickNextMultipleTimes(numberOfClicks){
    for(var i = 0; i < numberOfClicks; i++) {
        cy.get('#next-button').click()
    }
}

describe('Start workout', () => {
    it('Goes to the home page and starts the workout', () => {
        goToSetsPage();

        verifyContentsOfSet({
            exerciseType: 'Squat',
            plannedReps: '5',
            plannedWeight: '80', 
            actualReps: '5', 
            actualWeight: '80'
        })
    })
    
    it('should have the previous button disabled', () => {
        goToSetsPage();
        cy.get('#previous-button').should('be.disabled')
    })

    it('should have the next button enabled', () => {
        goToSetsPage();
        cy.get('#next-button').should('not.be.disabled')
    })

    it('should move to the next set when next is clicked', () => {
        goToSetsPage();

        cy.get('#next-button').click()
        verifyContentsOfSet({
            exerciseType: 'Squat',
            plannedReps: '5',
            plannedWeight: '80', 
            actualReps: '5', 
            actualWeight: '80'
        })
    })

    it('should have previous button enabled when moved to next set', () => {
        goToSetsPage();
        cy.get('#next-button').click()
        cy.get('#previous-button').should('not.be.disabled')
    })

    it('should have disabled next button when at end of workout', () => {
        goToSetsPage();
        clickNextMultipleTimes(14)

        cy.get('#next-button').should('be.disabled')
    })

    it('should move to the previous set when previous is clicked', () => {
        goToSetsPage();

        clickNextMultipleTimes(5)
        verifyContentsOfSet({
            exerciseType: 'BenchPress',
            plannedReps: '5',
            plannedWeight: '50', 
            actualReps: '5', 
            actualWeight: '50'
        })

        cy.get('#previous-button').click()
        verifyContentsOfSet({
            exerciseType: 'Squat',
            plannedReps: '5',
            plannedWeight: '80', 
            actualReps: '5', 
            actualWeight: '80'
        })
    })
})

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
            exerciseType: 'BenchPress',
            plannedReps: '12',
            plannedWeight: '57.5', 
            actualReps: '12', 
            actualWeight: '57.5'
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
            exerciseType: 'BenchPress',
            plannedReps: '10',
            plannedWeight: '57.5', 
            actualReps: '10', 
            actualWeight: '57.5'
        })
    })

    it('should have previous button enabled when moved to next set', () => {
        goToSetsPage();
        cy.get('#next-button').click()
        cy.get('#previous-button').should('not.be.disabled')
    })

    it('should have disabled next button when at end of workout', () => {
        goToSetsPage();
        clickNextMultipleTimes(8)

        cy.get('#next-button').should('be.disabled')
    })

    it('should move to the previous set when previous is clicked', () => {
        goToSetsPage();

        clickNextMultipleTimes(3)
        verifyContentsOfSet({
            exerciseType: 'BentOverRow',
            plannedReps: '12',
            plannedWeight: '52.5', 
            actualReps: '12', 
            actualWeight: '52.5'
        })

        cy.get('#previous-button').click()
        verifyContentsOfSet({
            exerciseType: 'BenchPress',
            plannedReps: '8',
            plannedWeight: '57.5', 
            actualReps: '8', 
            actualWeight: '57.5'
        })
    })
})
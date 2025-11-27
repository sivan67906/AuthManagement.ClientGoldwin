'use strict';

(function () {
    const wizardValidation = document.querySelector('#wizard-validation');
    if (wizardValidation) {

        const wizardValidationForm = wizardValidation.querySelector('#wizard-validation-form');
        const wizardValidationFormStep1 = wizardValidationForm.querySelector('#account-details-validation');
        const wizardValidationFormStep2 = wizardValidationForm.querySelector('#personal-info-validation');
        const wizardValidationFormStep3 = wizardValidationForm.querySelector('#social-links-validation');

        const wizardValidationNext = [].slice.call(wizardValidationForm.querySelectorAll('.btn-next'));
        const wizardValidationPrev = [].slice.call(wizardValidationForm.querySelectorAll('.btn-prev'));

        const validationStepper = new Stepper(wizardValidation, { linear: true });

        // Step 1 Validation
        const FormValidation1 = FormValidation.formValidation(wizardValidationFormStep1, {
            fields: {
                formValidationUsername: {
                    validators: {
                        notEmpty: { message: 'Username is required' },
                        stringLength: { min: 6, max: 30, message: '6–30 characters allowed' },
                        regexp: { regexp: /^[a-zA-Z0-9 ]+$/, message: 'Only letters, numbers and spaces allowed' }
                    }
                },
                formValidationEmail: {
                    validators: {
                        notEmpty: { message: 'Email is required' },
                        emailAddress: { message: 'Enter a valid email address' }
                    }
                },
                formValidationPass: {
                    validators: { notEmpty: { message: 'Password is required' } }
                },
                formValidationConfirmPass: {
                    validators: {
                        notEmpty: { message: 'Confirm password is required' },
                        identical: {
                            compare: function () {
                                return wizardValidationFormStep1.querySelector('[name="formValidationPass"]').value;
                            },
                            message: 'Passwords do not match'
                        }
                    }
                }
            },
            plugins: {
                trigger: new FormValidation.plugins.trigger(),
                bootstrap5: new FormValidation.plugins.Bootstrap5({
                    rowSelector: '.form-control-validation'
                }),
                autoFocus: new FormValidation.plugins.AutoFocus(),
                submitButton: new FormValidation.plugins.SubmitButton()
            }
        }).on('core.form.valid', function () {
            validationStepper.next();
        });

        // Step 2 Validation
        const FormValidation2 = FormValidation.formValidation(wizardValidationFormStep2, {
            fields: {
                formValidationFirstName: { validators: { notEmpty: { message: 'First name required' } } },
                formValidationLastName: { validators: { notEmpty: { message: 'Last name required' } } },
                formValidationCountry: { validators: { notEmpty: { message: 'Country required' } } },
                formValidationLanguage: { validators: { notEmpty: { message: 'Language required' } } }
            },
            plugins: {
                trigger: new FormValidation.plugins.Trigger(),
                bootstrap5: new FormValidation.plugins.Bootstrap5({
                    rowSelector: '.form-control-validation'
                }),
                autoFocus: new FormValidation.plugins.AutoFocus(),
                submitButton: new FormValidation.plugins.SubmitButton()
            }
        }).on('core.form.valid', function () {
            validationStepper.next();
        });

        // Step 3 Validation
        const FormValidation3 = FormValidation.formValidation(wizardValidationFormStep3, {
            fields: {
                formValidationTwitter: {
                    validators: {
                        notEmpty: { message: 'Twitter URL required' },
                        uri: { message: 'Enter a valid URL' }
                    }
                },
                formValidationFacebook: {
                    validators: {
                        notEmpty: { message: 'Facebook URL required' },
                        uri: { message: 'Enter a valid URL' }
                    }
                },
                formValidationGoogle: {
                    validators: {
                        notEmpty: { message: 'Google URL required' },
                        uri: { message: 'Enter a valid URL' }
                    }
                },
                formValidationLinkedIn: {
                    validators: {
                        notEmpty: { message: 'LinkedIn URL required' },
                        uri: { message: 'Enter a valid URL' }
                    }
                }
            },
            plugins: {
                trigger: new FormValidation.plugins.Trigger(),
                bootstrap5: new FormValidation.plugins.Bootstrap5({
                    rowSelector: '.form-control-validation'
                }),
                autoFocus: new FormValidation.plugins.AutoFocus(),
                submitButton: new FormValidation.plugins.SubmitButton()
            }
        }).on('core.form.valid', function () {
            alert('✅ Form submitted successfully!');
        });

        // Next & Previous button handlers
        wizardValidationNext.forEach(btn => {
            btn.addEventListener('click', () => {
                switch (validationStepper._currentIndex) {
                    case 0: FormValidation1.validate(); break;
                    case 1: FormValidation2.validate(); break;
                    case 2: FormValidation3.validate(); break;
                }
            });
        });
        wizardValidationPrev.forEach(btn => {
            btn.addEventListener('click', () => {
                validationStepper.previous();
            });
        });
    }
})();
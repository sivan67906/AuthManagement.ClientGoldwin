/**
 * Pages Authentication
 */
'use strict';

let authObserver = null;

function setupPasswordEye() {
    document.querySelectorAll('.password-eye').forEach(btn => {
        const newBtn = btn.cloneNode(true);
        btn.replaceWith(newBtn);
        newBtn.addEventListener('click', function () {
            const input = this.previousElementSibling;
            if (!input) return;
            const icon = this.querySelector('i');
            const isPassword = input.type === 'password';
            input.type = isPassword ? 'text' : 'password';
            icon.classList.toggle('tabler-eye');
            icon.classList.toggle('tabler-eye-off');
        });
    });
}

function initPagesAuth() {
    setupPasswordEye();

    const formAuthentication = document.querySelector('#formAuthentication');
    if (!formAuthentication || typeof FormValidation === 'undefined') return;

    // FormValidation setup
    FormValidation.formValidation(formAuthentication, {
        fields: {
            username: {
                validators: {
                    notEmpty: { message: 'Please enter username' },
                    stringLength: { min: 6, message: 'Username must be more than 6 characters' }
                }
            },
            email: {
                validators: {
                    notEmpty: { message: 'Please enter your email' },
                    emailAddress: { message: 'Please enter a valid email address' }
                }
            },
            'email-username': {
                validators: {
                    notEmpty: { message: 'Please enter email / username' },
                    stringLength: { min: 6, message: 'Username must be more than 6 characters' }
                }
            },
            password: {
                validators: {
                    notEmpty: { message: 'Please enter your password' },
                    stringLength: { min: 6, message: 'Password must be more than 6 characters' }
                }
            },
            'confirm-password': {
                validators: {
                    notEmpty: { message: 'Please confirm password' },
                    identical: {
                        compare: () => formAuthentication.querySelector('[name="password"]').value,
                        message: 'The password and its confirmation do not match'
                    },
                    stringLength: { min: 6, message: 'Password must be more than 6 characters' }
                }
            },
            terms: {
                validators: {
                    notEmpty: { message: 'Please agree to terms & conditions' }
                }
            }
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger(),
            bootstrap5: new FormValidation.plugins.Bootstrap5({
                eleValidClass: '',
                rowSelector: '.form-control-validation'
            }),
            submitButton: new FormValidation.plugins.SubmitButton(),
            defaultSubmit: new FormValidation.plugins.DefaultSubmit(),
            autoFocus: new FormValidation.plugins.AutoFocus()
        },
        init: instance => {
            instance.on('plugins.message.placed', e => {
                const parent = e.element.parentElement;

                // Check if this message is already present
                const existingMessages = Array.from(parent.querySelectorAll('.input-group'));
                if (!existingMessages.includes(e.messageElement)) {
                    if (parent.classList.contains('input-group')) {
                        // Insert after input-group wrapper
                        if (!parent.nextElementSibling || !parent.nextElementSibling.isEqualNode(e.messageElement)) {
                            parent.insertAdjacentElement('afterend', e.messageElement);
                        }
                    } else {
                        // Normal input
                        parent.appendChild(e.messageElement);
                    }
                }
            });
        }
    });
 
    // Numeral-mask input
    const numeralMaskElements = document.querySelectorAll('.numeral-mask');
    numeralMaskElements.forEach(el => {
        el.addEventListener('input', event => {
            el.value = event.target.value.replace(/\D/g, ''); // keep only digits
        });
    });
}
function destroyPagesAuth() {
    if (authObserver) {
        authObserver.disconnect();
        authObserver = null;
    }
}

window.destroyPagesAuth = destroyPagesAuth;

using scbwi2017.Models.RegistrationViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scbwi2017.Models.Data
{
    public class Registration : Common
    {
        public ApplicationUser user { get; set; }
        public RegistrationType type { get; set; }
        public bool takingbus { get; set; }
        public Meal meal { get; set; }
        public virtual ICollection<Extra> Extras { get; set; }
        public Workshop first { get; set; }
        public Workshop second { get; set; }
        public int manuscript { get; set; }
        public int portfolio { get; set; }
        public int satdinner { get; set; }
        public Coupon coupon { get; set; }

        public DateTime paid { get; set; }
        public DateTime cleared { get; set; }

        public decimal subtotal { get; set; }
        public decimal total { get; set; }

        public string paypalid { get; set; }

        public void SetPayPalId()
        {
            paypalid = $"{user.firstname}{user.lastname}{created.Ticks}";
        }

        public Registration() { }

        public Registration(RegistrationViewModel r)
        {
            user = new ApplicationUser();

            user.firstname = r.user.first;
            user.lastname = r.user.last;
            user.phone = r.user.phone;
            user.postalcode = r.user.zip;
            user.address1 = r.user.address1;
            user.address2 = r.user.address2;
            user.city = r.user.city;
            user.Email = r.user.email;
            user.UserName = r.user.email;
            user.state = r.user.state;
        }

        public FlatRegistration Flatten() => new FlatRegistration(this);

        public string GenEmail()
        {
            var flat = Flatten();

            var email = $@"<h3>Congratulations! We have received your registration.</h3> 
<p>If you have any questions, please don't hesitate to contact us at <a href='mailto:florida-ra@scbwi.org'>florida-ra@scbwi.org</a></p>
<p>For your records, here what you requested:</p>
<table>
    <tbody>
        <tr>
            <td><b>Name</b></td>
            <td>{flat.firstname} {flat.lastname}</td>
            <td><b>Registration Chosen</b></td>
            <td>{flat.registrationtype}</td>
        </tr>
        <tr>
            <td><b>Address</b></td>
            <td>
                {flat.address1}<br />
                {flat.address2}<br />
                {flat.city}, {flat.state} {flat.postalcode}<br />
            </td>
            <td><b>Comprehensive Chosen:</b></td>
            <td>{flat.comprehensive}</td>
        </tr>
        <tr>
            <td><b>Email:</b></td>
            <td>{flat.Email}</td>
            <td><b>Workshops Chosen:</b></td>
            <td>
                {flat.first_workshop}<br />
                {flat.second_workshop}
            </td>
        </tr>
        <tr>
            <td><b>Phone:</b></td>
            <td>{flat.phone}</td>
            <td><b>Critiques requested:</b></td>
            <td>
                Manuscript: {flat.manuscript}<br />
                Portfolio: {flat.portfolio}<br />
            </td>
        </tr>
        <tr>
            <td><b>Saturday Night Dinner Chosen:</b></td>
            <td>{flat.meal}</td>
            <td><b>Are you taking the bus friday night?</b></td>
            <td>{flat.takingbus}</td>
        </tr>
        <tr>
            <td><b>Any extra Saturday Dinner Tickets?</b></td>
            <td>{flat.satdinner}</td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td><b>Subtotal:</b></td>
            <td>{flat.subtotal}</td>
            <td><b>Total:</b></td>
            <td>{flat.total}</td>
        </tr>
    </tbody>
</table>";

            return email;
        }
    }

    public class FlatRegistration
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postalcode { get; set; }
        public string phone { get; set; }
        public string Email { get; set; }
        public string registrationtype { get; set; }
        public string takingbus { get; set; }
        public string meal { get; set; }
        public string comprehensive { get; set; }
        public string first_workshop { get; set; }
        public string second_workshop { get; set; }
        public int manuscript { get; set; }
        public int portfolio { get; set; }
        public int satdinner { get; set; }
        public string coupon { get; set; }
        public string created { get; set; }
        public string paid { get; set; }
        public string cleared { get; set; }
        public decimal subtotal { get; set; }
        public decimal total { get; set; }

        public FlatRegistration(Registration r)
        {
            firstname = r.user.firstname;
            lastname = r.user.lastname;
            address1 = r.user.address1;
            address2 = r.user.address2;
            city = r.user.city;
            state = r.user.state;
            postalcode = r.user.postalcode;
            phone = r.user.phone;
            registrationtype = r.type.title;
            takingbus = r.takingbus ? "Yes" : "No";
            meal = r.meal.title;
            comprehensive = r.Extras?.FirstOrDefault()?.title ?? "None";
            first_workshop = r.first?.title ?? "None";
            second_workshop = r.second?.title ?? "None";
            coupon = r.coupon?.text;
            created = r.created.ToString();
            paid = r.paid.ToString();
            cleared = r.cleared.ToString();
            subtotal = r.subtotal;
            total = r.total;
            Email = r.user.Email;
            satdinner = r.satdinner;
        }
    }
}

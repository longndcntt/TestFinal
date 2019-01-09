<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:prism="clr-namespace:Prism.Mvvm;assembly=Prism.Forms"
             prism:ViewModelLocator.AutowireViewModel="True"
             xmlns:component="clr-namespace:CustomControl.CustomControl"
             x:Class="CustomControl.Views.VisitorPage"
             NavigationPage.HasNavigationBar="False">
    <ContentPage.Resources>
        <ResourceDictionary>
            <Style TargetType="component:CustomCheckBox">
                <Setter Property="OutlineColor" Value="Gray"></Setter>
          
            </Style>
        </ResourceDictionary>
    </ContentPage.Resources>
    <StackLayout >
        <Grid BackgroundColor="#e4e4e4" Margin="5">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="1"></ColumnDefinition>
                <ColumnDefinition Width="*"></ColumnDefinition>
                <ColumnDefinition Width="*"></ColumnDefinition>
                <ColumnDefinition Width="1"></ColumnDefinition>
            </Grid.ColumnDefinitions>
            <Grid.RowDefinitions>
                <RowDefinition Height="50"></RowDefinition>
                <RowDefinition Height="45"></RowDefinition>
                <RowDefinition Height="*"></RowDefinition>
                <RowDefinition Height="*"></RowDefinition>
                <RowDefinition Height="*"></RowDefinition>
                <RowDefinition Height="*"></RowDefinition>
                <RowDefinition Height="*"></RowDefinition>
                <RowDefinition Height="5"></RowDefinition>
                <RowDefinition Height="1"></RowDefinition>
            </Grid.RowDefinitions>
            <Grid Grid.Row="0" Grid.ColumnSpan="4" BackgroundColor="#666666">
                <Label FontSize="Large" VerticalOptions="Center"  Text="Company" TextColor="White" Margin=""></Label>
            </Grid>
            <component:CustomCheckBox Grid.Column="2" Grid.Row="2" DefaultText="Prospect"></component:CustomCheckBox>
            <component:CustomCheckBox Grid.Column="1" Grid.Row="2" DefaultText="Customer"></component:CustomCheckBox>
            <component:CustomCheckBox Grid.Column="1" Grid.Row="3" DefaultText="Competitor"></component:CustomCheckBox>
            <component:CustomCheckBox Grid.Column="2" Grid.Row="3" DefaultText="Supplier"></component:CustomCheckBox>
            <component:CustomCheckBox Grid.Column="1" Grid.Row="4" DefaultText="Agent"></component:CustomCheckBox>
            <component:CustomCheckBox Grid.Column="2" Grid.Row="4" DefaultText="Proj. Integrator/Dev."></component:CustomCheckBox>
            <component:CustomCheckBox Grid.Column="1" Grid.Row="5" DefaultText="Media/Design"></component:CustomCheckBox>
            <component:CustomCheckBox Grid.Column="2" Grid.Row="5" DefaultText="Other"></component:CustomCheckBox>
            <Entry Grid.Column="2" Grid.Row="6"></Entry>

            <BoxView BackgroundColor="Black"  Grid.Row="1" Grid.RowSpan="8" Grid.Column="0"></BoxView>
            <BoxView BackgroundColor="Black" Grid.Row="1" Grid.RowSpan="8" Grid.Column="3"></BoxView>

            <BoxView BackgroundColor="Black" Grid.Row="8" Grid.ColumnSpan="4"></BoxView>
        </Grid>
    </StackLayout>
</ContentPage>